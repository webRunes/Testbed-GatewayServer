using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;

namespace Demo.Data
{
    public class StorageSingleton
    {
        private static readonly StorageSingleton instance;

        static StorageSingleton()
        {
            instance = new StorageSingleton();
        }
        // Mark constructor as private as no one can create it but itself.
        private StorageSingleton()
        {
            // For constructing
        }

        // The only way to access the created instance.
        public static StorageSingleton Instance
        {
            get
            {
                return instance;
            }
        }

        // Note that this will be null when the instance if not set to
        // something in the constructor.
        public DeviceData DeviceData { get; set; }
        public AppSettings AppSettings { get; set; } = new AppSettings();
    }
}
