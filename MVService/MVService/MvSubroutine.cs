using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVService
{
    public class MvSubroutine
    {
        private int? _numOfParam;


        public MvSubroutine(string subroutineName, int numberOfParameters, string[] callParameters)
        {
            ReturnParameters = null;
            LastException = null;
            Name = subroutineName;
            _numOfParam = numberOfParameters;
            this.CallParameters = callParameters;
        }
        public MvSubroutine(string subroutineName, string[] callParameters)
        {
            ReturnParameters = null;
            LastException = null;
            Name = subroutineName;
            this.CallParameters = callParameters;
        }
        public MvSubroutine(string subroutineName, int numberOfParameters)
        {
            CallParameters = null;
            ReturnParameters = null;
            LastException = null;
            Name = subroutineName;
            _numOfParam = numberOfParameters;
        }
        public MvSubroutine(string subroutineName)
        {
            CallParameters = null;
            ReturnParameters = null;
            LastException = null;
            Name = subroutineName;
        }

        public int NumberOfParameters
        {
            get
            {
                if (_numOfParam != null)
                {
                    return (int)_numOfParam;
                }
                return CallParameters.Length;
            }
            set => _numOfParam = value;
        }

        public string[] ReturnParameters { get; set; }


        public string[] CallParameters { get; }

        public string Name { get; }

        public Exception LastException { get; set; }
    }
}
