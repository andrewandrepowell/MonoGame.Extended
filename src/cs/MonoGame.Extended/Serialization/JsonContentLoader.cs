using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Content;
using Newtonsoft.Json;

namespace MonoGame.Extended.Serialization
{
    public class JsonContentLoader : IContentLoader
    {
        private static Dictionary<string, object> contentCache = new();
        public T Load<T>(ContentManager contentManager, string path)
        {
            if (contentCache.ContainsKey(path))            
                return (T)contentCache[path];            
            using (var stream = contentManager.OpenStream(path))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new MonoGameJsonSerializer(contentManager, path);
                T content = serializer.Deserialize<T>(jsonReader);
                contentCache[path] = content;
                return content;
            }
        }
    }
}
