using System;
using System.Reflection;

namespace MilNet.Core.Attributes
{
    /// <summary>Localized attribute</summary>
    public abstract class LocalizedAttribute : Attribute
    {
        #region Member Fields

        private string message;
        private Func<string> messageResourceAccessor;
        private string messageResourceName;
        private Type messageResourceType;
        private string defaultMessage;

        #endregion
        #region Constructors

        /// <summary>Default constructor for any localized attribute</summary>
        /// <remarks>This constructor chooses an empty message. Developers subclassing LocalizedAttribute should use other constructors or supply a better message</remarks>
        protected LocalizedAttribute() : this(() => null) { }

        /// <summary>Constructor that accepts a fixed message</summary>
        /// <param name="message">A non-localized message to use in <see cref="MessageString"/></param>
        protected LocalizedAttribute(string message) : this(() => message) { }

        /// <summary>Allows for providing a resource accessor function that will be used by the <see cref="MessageString"/>property to retrieve the message</summary>
        /// <param name="messageAccessor">The <see cref="Func{T}"/> that will return a message</param>
        protected LocalizedAttribute(Func<string> messageAccessor)
        {
            messageResourceAccessor = messageAccessor; // If null, will later be exposed as lack of message to be able to construct accessor
        }

        #endregion
        #region Internal Properties
        /// <summary>Default message string. This message will be used if the user has not set <see cref="Message"/> or the <see cref="MessageResourceType"/> and <see cref="MessageResourceName"/> pair</summary>
        internal string DefaultMessage
        {
            get
            {
                return defaultMessage;
            }
            set
            {
                defaultMessage = value;
                messageResourceAccessor = null;
                CustomMessageSet = true;
            }
        }

        #endregion
        #region Protected Properties

        /// <summary>Localized message string, coming either from <see cref="Message"/>, or from evaluating the <see cref="MessageResourceType"/> and <see cref="MessageResourceName"/> pair</summary>
        protected string MessageString
        {
            get
            {
                SetupResourceAccessor();
                return messageResourceAccessor();
            }
        }

        /// <summary>Flag indicating whether a developer has customized the attribute's message by setting any one of <see cref="Message"/>, <see cref="MessageResourceName"/>, <see cref="MessageResourceType"/> or <see cref="DefaultMessage"/></summary>
        internal bool CustomMessageSet { get; private set; }

        #endregion
        #region Public Properties

        /// <summary>Explicit message string</summary>
        /// <value>This property is intended to be used for non-localizable error messages Use <see cref="MessageResourceType"/> and <see cref="MessageResourceName"/> for localizable messages</value>
        public string Message
        {
            get
            {
                return message ?? defaultMessage;
            }
            set
            {
                message = value;
                messageResourceAccessor = null;
                CustomMessageSet = true;

                // Explicitly setting Message also sets DefaultMessage if null.
                // This prevents subsequent read of Message from returning default.
                if (value == null)
                    defaultMessage = null;
            }
        }

        /// <summary>Resource name (property name) to use as the key for lookups on the resource type</summary>
        /// <value>Use this property to set the name of the property within <see cref="MessageResourceType"/> that will provide a localized error message. Use <see cref="Message"/> for non-localized error messages</value>
        public string MessageResourceName
        {
            get
            {
                return messageResourceName;
            }
            set
            {
                messageResourceName = value;
                messageResourceAccessor = null;
                CustomMessageSet = true;
            }
        }

        /// <summary>Resource type to use for error message lookups</summary>
        /// <value>Use this property only in conjunction with <see cref="MessageResourceName"/>. They are used together to retrieve localized messages at runtime
        /// <para>Use <see cref="Message"/> instead of this pair if messages are not localized</para>
        /// </value>
        public Type MessageResourceType
        {
            get
            {
                return messageResourceType;
            }
            set
            {
                messageResourceType = value;
                messageResourceAccessor = null;
                CustomMessageSet = true;
            }
        }

        #endregion
        #region Private Methods

        /// <summary>Validates the configuration of this attribute and sets up the appropriate string accessor. This method bypasses all verification once the ResourceAccessor has been set.</summary>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        private void SetupResourceAccessor()
        {
            if (messageResourceAccessor == null)
            {
                string localMessage = Message;
                bool resourceNameSet = !string.IsNullOrEmpty(messageResourceName);
                bool messageSet = !string.IsNullOrEmpty(message);
                bool resourceTypeSet = messageResourceType != null;
                bool defaultMessageSet = !string.IsNullOrEmpty(defaultMessage);

                // The following combinations are illegal and throw InvalidOperationException:
                //   1) Both ErrorMessage and ErrorMessageResourceName are set, or
                //   2) None of ErrorMessage, ErrorMessageReourceName, and DefaultErrorMessage are set
                if ((resourceNameSet && messageSet) || !(resourceNameSet || messageSet || defaultMessageSet))
                    throw new InvalidOperationException();

                // Must set both or neither of MessageResourceType and MessageResourceName
                if (resourceTypeSet != resourceNameSet)
                    throw new InvalidOperationException();

                // If set resource type (and we know resource name too), then go setup the accessor
                if (resourceNameSet)
                    SetResourceAccessorByPropertyLookup();
                else
                {
                    // Here if not using resource type/name -- the accessor is just the message string, which we know is not empty to have gotten this far
                    messageResourceAccessor = delegate {
                        return localMessage; // We captured error message to local in case it changes before accessor runs
                    };
                }
            }
        }

        /// <summary>Set the resource accessor from the property lookup</summary>
        /// <exception cref="InvalidOperationException"/>
        private void SetResourceAccessorByPropertyLookup()
        {
            if (messageResourceType == null || string.IsNullOrEmpty(messageResourceName))
                throw new InvalidOperationException();

            PropertyInfo property = messageResourceType.GetProperty(messageResourceName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (property != null)
            {
                MethodInfo propertyGetter = property.GetGetMethod(nonPublic: true);
                // We only support internal and public properties
                if (propertyGetter == null || (!propertyGetter.IsAssembly && !propertyGetter.IsPublic))
                    property = null; // Set the property to null so the exception is thrown as if the property wasn't found
            }
            if (property == null)
                throw new InvalidOperationException();
            if (property.PropertyType != typeof(string))
                throw new InvalidOperationException();

            messageResourceAccessor = delegate {
                return (string)property.GetValue(null, null);
            };
        }

        #endregion
    }
}
