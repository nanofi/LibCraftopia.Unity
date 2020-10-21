

using LibCraftopia.Unity.Editor.Configuration.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace LibCraftopia.Unity.Editor.Configuration
{
    public class ConfigurationPipeline : IList<IConfigureTask>
    {
        private List<IConfigureTask> tasks = new List<IConfigureTask>();
        public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

        public static ConfigurationPipeline CreateDefaultPipeline()
        {
            var pipeline = new ConfigurationPipeline();
            pipeline.Add(new ExtractParameters());
            pipeline.Add(new MakeDirectory());
            pipeline.Add(new ResolveDependence());
            pipeline.Add(new DownloadDependences());
            pipeline.Add(new GenerateAsmdef());
            pipeline.Add(new GenerateRes());
            pipeline.Add(new GenerateBasePluginCs());
            pipeline.Add(new GenerateAsmInfo());
            pipeline.Add(new RefreshAsset());
            return pipeline;
        }

        public void Execute()
        {
            Parameters.Add("Parameters", new ConfigurationParameters());
            Parameters.Add("AssemblyDependences", new AssemblyDependences());
            foreach (var task in this)
            {
                var taskType = task.GetType();
                foreach (var kv in Parameters)
                {
                    var name = kv.Key;
                    var value = kv.Value;
                    var field = taskType.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (field != null && field.FieldType.IsAssignableFrom(value.GetType()))
                    {
                        field.SetValue(task, value);
                    }
                }
                task.Invoke();
            }
            Parameters.Clear();
        }

        #region IList
        public IConfigureTask this[int index] { get => tasks[index]; set => tasks[index] = value; }

        public int Count => tasks.Count;

        public bool IsReadOnly => false;

        public void Add(IConfigureTask item)
        {
            tasks.Add(item);
        }

        public void Clear()
        {
            tasks.Clear();
        }

        public bool Contains(IConfigureTask item)
        {
            return tasks.Contains(item);
        }

        public void CopyTo(IConfigureTask[] array, int arrayIndex)
        {
            tasks.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IConfigureTask> GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        public int IndexOf(IConfigureTask item)
        {
            return tasks.IndexOf(item);
        }

        public void Insert(int index, IConfigureTask item)
        {
            tasks.Insert(index, item);
        }

        public bool Remove(IConfigureTask item)
        {
            return tasks.Remove(item);
        }

        public void RemoveAt(int index)
        {
            tasks.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tasks.GetEnumerator();
        }
        #endregion
    }
}