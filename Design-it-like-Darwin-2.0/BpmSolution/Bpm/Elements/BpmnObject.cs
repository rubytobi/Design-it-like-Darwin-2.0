namespace Bpm.NotationElements
{
    /// <summary>
    /// </summary>
    public class BpmnObject
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        public BpmnObject(string name, string type, bool processInput, bool processOutput, double price)
        {
            Name = name;
            Type = type;
            ProcessInput = processInput;
            ProcessOutput = processOutput;
            Price = price;
        }

        public string Name { get; }
        public string Type { get; }
        public bool ProcessInput { get; }
        public bool ProcessOutput { get; }
        public double Price { get; }

        public override bool Equals(object o)
        {
            if (o is BpmnObject)
            {
                var other = o as BpmnObject;
                return Name.Equals(other.Name);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual BpmnObject Clone()
        {
            return new BpmnObject(Name, Type, ProcessInput, ProcessOutput, Price);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Name.GetHashCode();
    }
}