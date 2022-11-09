using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreCore.Entity
{
    /// <summary>
    /// method yanıtları bu obje üzerinden yürüyecek
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public int RecordCount { get; set; }
    }
}
