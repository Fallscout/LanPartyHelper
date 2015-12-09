using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanPartyServer
{
    public class Model
    {
        private Model()
        {

        }

        private static Model instance;
        public static Model Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Model();
                }
                return instance;
            }
        }
    }
}
