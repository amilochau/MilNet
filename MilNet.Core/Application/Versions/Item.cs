using MilNet.Core.Types;

namespace MilNet.Core.Application.Versions
{
    /// <summary>Version detail item type</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public enum TypeItem
    {
        /// <summary>Unknown</summary>
        Unknown = -1,
        /// <summary>Bug correction</summary>
        BugCorrection = 0,
        /// <summary>New feature</summary>
        NewFeature = 1,
        /// <summary>Operation change</summary>
        OperationChange = 2,
        /// <summary>Deleted feature</summary>
        DeletedFeature = 3
    }

    /// <summary>Version detail item</summary>
    /// <remarks>TODO Review (protected) set</remarks>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class Item : ReadableEnum<TypeItem>
    {
        /// <summary>Item description</summary>
        public string Description { get; set; }

        /// <summary>Constructor</summary>
        public Item() => Value = TypeItem.Unknown;
        
        /// <summary>Convert to string</summary>
        public override string ToString() => Write(Value);
        /// <summary>Read as string</summary>
        public override void FromString(string text) => Value = Read(text);
        
        /// <summary>Ecriture d'un type d'item énumérée en string</summary>
        public static string Write(TypeItem type)
        {
            switch (type)
            {
                case TypeItem.BugCorrection:
                    return Resources.Resources.BugCorrection;
                case TypeItem.NewFeature:
                    return Resources.Resources.NewFeature;
                case TypeItem.OperationChange:
                    return Resources.Resources.OperationChange;
                case TypeItem.DeletedFeature:
                    return Resources.Resources.DeletedFeature;
                default:
                    return "";
            }
        }
        /// <summary>Read an item type as string</summary>
        public static TypeItem Read(string type)
        {
            if (string.IsNullOrEmpty(type))
                return TypeItem.Unknown;

            switch (type)
            {
                case "0":
                    return TypeItem.BugCorrection;
                case "1":
                    return TypeItem.NewFeature;
                case "2":
                    return TypeItem.OperationChange;
                case "3":
                    return TypeItem.DeletedFeature;
                default:
                    return TypeItem.Unknown;
            }
        }
    }
}
