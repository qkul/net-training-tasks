using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AsyncIO
{
    public static class Tasks
    {
        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the synchronous way and can be used to compare the performace of sync \ async approaches. 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContent(this IEnumerable<Uri> uris)
        {
            // TODO : Implement GetUrlContent 
            return uris.Select(uri => new WebClient().DownloadString(uri));
        }

        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the asynchronous way and can be used to compare the performace of sync \ async approaches. 
        /// 
        /// maxConcurrentStreams parameter should control the maximum of concurrent streams that are running at the same time (throttling). 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <param name="maxConcurrentStreams">Max count of concurrent request streams</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContentAsync(this IEnumerable<Uri> uris, int maxConcurrentStreams)
        {
            //  TODO: Implement GetUrlContentAsync
            //var enumer = uris.GetEnumerator();
            //var tasks = new List<Task<string>>();  
            //while (tasks.Count() < maxConcurrentStreams && enumer.MoveNext())
            //    tasks.Add((new WebClient().DownloadStringTaskAsync(enumer.Current)));

            var tasks = new Task<string>[maxConcurrentStreams];
            int id = 0;
            foreach (var uri in uris)
            {
                if (id >= maxConcurrentStreams)
                {
                    var idCompletedTask = Task.WaitAny(tasks);
                    var completedTask = tasks[idCompletedTask];
                    tasks[idCompletedTask] = GetOneUrl(uri);
                    yield return completedTask.Result;
                }
                else
                {
                    tasks[id] = GetOneUrl(uri);
                    id++;
                }
            }
      
            var listTasks = tasks.ToList();
            while (!listTasks.Any())
            {
                var idTask = Task.WaitAny(listTasks.ToArray());
                var resultTask = listTasks[idTask];
                listTasks.RemoveAt(idTask);
                yield return resultTask.Result;
            }
        }
        private static Task<string> GetOneUrl(Uri uri)
        {
            return new HttpClient().GetStringAsync(uri);
        }

        /// <summary>
        /// Calculates MD5 hash of required resource.
        /// 
        /// Method has to run asynchronous. 
        /// Resource can be any of type: http page, ftp file or local file.
        /// </summary>
        /// <param name="resource">Uri of resource</param>
        /// <returns>MD5 hash</returns>
        public static async  Task<string> GetMD5Async(this Uri resource)
        {
          //  TODO: Implement GetMD5Async
            var res = MD5.Create().ComputeHash(await new WebClient().DownloadDataTaskAsync(resource))
                 .Select<byte, string>(x => x.ToString("x2").ToLower()).ToArray();
            return string.Concat(res);
        }
    }
}
