using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Serialization.Tasks
{
    // TODO : Make Company class xml-serializable using DataContractSerializer 
    // Employee.Manager should be serialized as reference
    // Company class has to be forward compatible with all derived versions


    [KnownType(typeof(Employee))]
    [KnownType(typeof(Manager))]
    [KnownType(typeof(Worker))]
    [DataContract]
    public class Company : IExtensibleDataObject
    {
        private ExtensionDataObject extensionDataObject;
        public ExtensionDataObject ExtensionData
        {
            get { return extensionDataObject; }
            set { extensionDataObject = value; }
        }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public IList<Employee> Employee { get; set; }
    }
    [KnownType(typeof(Manager))]
    [KnownType(typeof(Worker))]
    [DataContract(IsReference = true)]//  сохранить данные ссылки на объект с помощью стандартного XML
    public abstract class Employee {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public Manager Manager { get; set; }
    }
    [DataContract]
    public class Worker : Employee {
        [DataMember]
        public int Salary { get; set; }
    }
    [DataContract]
    public class Manager : Employee {
        [DataMember]
        public int YearBonusRate { get; set; } 
    }

}
