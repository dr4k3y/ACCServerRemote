using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;

namespace Slave.Controllers
{
    public class ServerController : ApiController
    {
        private string accServerPath = @"C:\Program Files (x86)\Steam\steamapps\common\Assetto Corsa Competizione Dedicated Server\server";
        private static NamedPipeClientStream client = new NamedPipeClientStream("Intercom");
        private string[] filenames = { "assistRules.json", "configuration.json", "event.json", "settings.json", "eventRules.json", "entrylist.json", "bop.json" };
        // GET: api/Server
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Server/settings.json
        public string Get(string filename)
        {
            return ReadCfg(filename);
        }

        // POST: api/Server
        public void Post([FromBody]string s)
        {
            Rura(s);
        }

        // PUT: api/Server/5
        public void Put(string filename, [FromBody]object content)
        {
            WriteCfg(filename, content.ToString());
        }

        // DELETE: api/Server/5
        public void Delete(int id)
        {
        }

        private void Rura(string s)
        {
            NamedPipeClientStream Pipe = new NamedPipeClientStream(".", "Interconnect", PipeDirection.Out);
            StreamWriter sr = new StreamWriter(Pipe);
            try
            {
                Pipe.Connect(1000);
                sr.WriteLine(s);
                sr.Flush();
            }
            catch (TimeoutException te)
            {
                Console.WriteLine("ERROR: {0}", te.Message);
            }
        }
        private string ReadCfg(string filename)
        {
            using (StreamReader sr = new StreamReader(accServerPath + @"\cfg\" + filename))
            {
                return sr.ReadToEnd();
            }
        }
        private void WriteCfg(string filename, string content)
        {
            if (filenames.Contains<string>(filename))
            {
                if (File.Exists(accServerPath + @"\cfg\" + filename) == false)
                {
                    File.Create(accServerPath + @"\cfg\" + filename).Close();
                }
                using (StreamWriter sw = new StreamWriter(accServerPath + @"\cfg\" + filename))
                {
                    sw.Write(content);
                    sw.Flush();
                }
            }
        }
    }
}
