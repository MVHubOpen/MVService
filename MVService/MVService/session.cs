using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using U2.Data.Client;
using U2.Data.Client.UO;

namespace MVService
{
    public class Session
    {
        public Session()
        {
            Account = WebConfigurationManager.AppSettings["DBAccount"];
            Hostname = WebConfigurationManager.AppSettings["DBHost"];
            Password = WebConfigurationManager.AppSettings["DBPassword"];
            Username = WebConfigurationManager.AppSettings["DBUser"];
            AuthenticatedDbUrlPath = WebConfigurationManager.AppSettings["AuthenticatedDbUrlPath"];
        }

    


        private U2Connection GetConnection()
        {
            var connectionString = new U2ConnectionStringBuilder
            {
                UserID = this.Username,
                Password = this.Password,
                Server = this.Hostname,
                Database = this.Account,
                AccessMode = "Native",
                RpcServiceType = "uvcs",
                ServerType = "UniVerse",
                Connect_Timeout = 1200
            }.ToString();

            var con = new U2Connection(connectionString);
            con.Open();
            return con;
        }

        private static void CloseConnection(U2Connection con)
        {
            con?.Close();
        }

        public bool Call(MvSubroutine subroutine)
        {
            var successful = false;
            if (subroutine == null) throw new ArgumentNullException(nameof(subroutine));
            U2Connection con;

            try
            {
                con = this.GetConnection();
            }
            catch (Exception ex)
            {
                throw new SessionException("Unable to Connect to Server", ex);
            }

            var dbSession = con.UniSession;
            dbSession.BlockingStrategy = UniObjectsTokens.UVT_WAIT_LOCKED;
            dbSession.LockStrategy = UniObjectsTokens.UVT_NO_LOCKS;
            dbSession.ReleaseStrategy = UniObjectsTokens.UVT_READ_RELEASE;
            dbSession.Timeout = 600;


            var dbSub = dbSession.CreateUniSubroutine(subroutine.Name, subroutine.NumberOfParameters);

            var paramCnt = 0;

            foreach (var param in subroutine.CallParameters)
            {
                if (paramCnt + 1 > subroutine.CallParameters.Length)
                {
                    break;
                }

                if (paramCnt + 1 > subroutine.NumberOfParameters)
                {
                    break;
                }

                dbSub.SetArg(paramCnt, param);
                paramCnt += 1;
            }

            try
            {
                dbSub.Call();

                subroutine.ReturnParameters = new string[subroutine.NumberOfParameters];
                var pCnt = subroutine.NumberOfParameters - 1;

                for (var i = 0; i <= pCnt; i++)
                {
                    subroutine.ReturnParameters[i] = dbSub.GetArg(i);
                }

                successful = true;

            }
            catch (Exception ex)
            {
                subroutine.LastException = ex;
                successful = false;
            }
            finally
            {
                CloseConnection(con);
            }


            return successful;
        }
        public string ReadString(string filename, string id)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            if (id == null) throw new ArgumentNullException(nameof(id));

            var con = this.GetConnection();
            var dbSession = con.UniSession;
            dbSession.BlockingStrategy = UniObjectsTokens.UVT_WAIT_LOCKED;
            dbSession.LockStrategy = UniObjectsTokens.UVT_NO_LOCKS;
            dbSession.ReleaseStrategy = UniObjectsTokens.UVT_READ_RELEASE;
            dbSession.Timeout = 6000;

            var file = dbSession.CreateUniFile(filename);
            var record = file.Read(id);
            var str = record.ToString();

            CloseConnection(con);

            return str;
        }

        public bool WriteString(string filename, string id, string record)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (record == null) record=string.Empty;

            throw new NotImplementedException();
        }
        protected string Hostname { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }
        protected string Account { get; set; }
        protected string ServiceType { get; set; }
        public string AuthenticatedDbUrlPath { get; set; }

    }
}
