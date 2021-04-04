using System.Collections.Generic;
using System.Linq;
using SCode.Client.Teacher.ConsoleApp.Domain.Repositories;

namespace SCode.Client.Teacher.ConsoleApp.Infrastructure.Repositories
{
    public class LogicPathRepository : ILogicPathRepository
    {
        private readonly Dictionary<int, string> _items;

        private int _lastId;

        public LogicPathRepository()
        {
            _items = new Dictionary<int, string>();

            _lastId = 0;
        }

        public string Get(int id)
        {
            return _items.TryGetValue(id, out var value)
                ? value
                : null;
        }

        public void Update(int id, string path)
        {
            _items[id] = path;
        }

        public int Add(string path)
        {
            if (_items.ContainsValue(path))
            {
                return _items
                    .First(x => x.Value == path)
                    .Key;
            }

            var id = _lastId++;

            _items.Add(id, path);

            return id;
        }
    }
}