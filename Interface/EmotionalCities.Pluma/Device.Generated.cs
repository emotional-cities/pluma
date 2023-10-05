using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace EmotionalCities.Pluma
{
    /// <summary>
    /// Generates events and processes commands for the Pluma device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the Pluma device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="Pluma"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 2110;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(Pluma);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(StreamsEnable) },
            { 33, typeof(StreamsDisable) },
            { 34, typeof(OximeterData) },
            { 35, typeof(EcgAndAnalogData) },
            { 36, typeof(GsrData) },
            { 37, typeof(AccelerometerData) },
            { 38, typeof(PortDigitalInput) },
            { 39, typeof(OutputSet) },
            { 40, typeof(OutputClear) }
        };
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="Pluma"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of Pluma messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="Pluma"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="Pluma"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="Pluma"/> device.
    /// </summary>
    /// <seealso cref="StreamsEnable"/>
    /// <seealso cref="StreamsDisable"/>
    /// <seealso cref="OximeterData"/>
    /// <seealso cref="EcgAndAnalogData"/>
    /// <seealso cref="GsrData"/>
    /// <seealso cref="AccelerometerData"/>
    /// <seealso cref="PortDigitalInput"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    [XmlInclude(typeof(StreamsEnable))]
    [XmlInclude(typeof(StreamsDisable))]
    [XmlInclude(typeof(OximeterData))]
    [XmlInclude(typeof(EcgAndAnalogData))]
    [XmlInclude(typeof(GsrData))]
    [XmlInclude(typeof(AccelerometerData))]
    [XmlInclude(typeof(PortDigitalInput))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [Description("Filters register-specific messages reported by the Pluma device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new StreamsEnable();
        }

        string INamedElement.Name
        {
            get => $"{nameof(Pluma)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the Pluma device.
    /// </summary>
    /// <seealso cref="StreamsEnable"/>
    /// <seealso cref="StreamsDisable"/>
    /// <seealso cref="OximeterData"/>
    /// <seealso cref="EcgAndAnalogData"/>
    /// <seealso cref="GsrData"/>
    /// <seealso cref="AccelerometerData"/>
    /// <seealso cref="PortDigitalInput"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    [XmlInclude(typeof(StreamsEnable))]
    [XmlInclude(typeof(StreamsDisable))]
    [XmlInclude(typeof(OximeterData))]
    [XmlInclude(typeof(EcgAndAnalogData))]
    [XmlInclude(typeof(GsrData))]
    [XmlInclude(typeof(AccelerometerData))]
    [XmlInclude(typeof(PortDigitalInput))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [XmlInclude(typeof(TimestampedStreamsEnable))]
    [XmlInclude(typeof(TimestampedStreamsDisable))]
    [XmlInclude(typeof(TimestampedOximeterData))]
    [XmlInclude(typeof(TimestampedEcgAndAnalogData))]
    [XmlInclude(typeof(TimestampedGsrData))]
    [XmlInclude(typeof(TimestampedAccelerometerData))]
    [XmlInclude(typeof(TimestampedPortDigitalInput))]
    [XmlInclude(typeof(TimestampedOutputSet))]
    [XmlInclude(typeof(TimestampedOutputClear))]
    [Description("Filters and selects specific messages reported by the Pluma device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new StreamsEnable();
        }

        string INamedElement.Name => $"{nameof(Pluma)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// Pluma register messages.
    /// </summary>
    /// <seealso cref="StreamsEnable"/>
    /// <seealso cref="StreamsDisable"/>
    /// <seealso cref="OximeterData"/>
    /// <seealso cref="EcgAndAnalogData"/>
    /// <seealso cref="GsrData"/>
    /// <seealso cref="AccelerometerData"/>
    /// <seealso cref="PortDigitalInput"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    [XmlInclude(typeof(StreamsEnable))]
    [XmlInclude(typeof(StreamsDisable))]
    [XmlInclude(typeof(OximeterData))]
    [XmlInclude(typeof(EcgAndAnalogData))]
    [XmlInclude(typeof(GsrData))]
    [XmlInclude(typeof(AccelerometerData))]
    [XmlInclude(typeof(PortDigitalInput))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [Description("Formats a sequence of values as specific Pluma register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new StreamsEnable();
        }

        string INamedElement.Name => $"{nameof(Pluma)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that enables the generation of events for the respective streams.
    /// </summary>
    [Description("Enables the generation of events for the respective streams.")]
    public partial class StreamsEnable
    {
        /// <summary>
        /// Represents the address of the <see cref="StreamsEnable"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="StreamsEnable"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="StreamsEnable"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StreamsEnable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Streams GetPayload(HarpMessage message)
        {
            return (Streams)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StreamsEnable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Streams> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Streams)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StreamsEnable"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StreamsEnable"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Streams value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StreamsEnable"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StreamsEnable"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Streams value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StreamsEnable register.
    /// </summary>
    /// <seealso cref="StreamsEnable"/>
    [Description("Filters and selects timestamped messages from the StreamsEnable register.")]
    public partial class TimestampedStreamsEnable
    {
        /// <summary>
        /// Represents the address of the <see cref="StreamsEnable"/> register. This field is constant.
        /// </summary>
        public const int Address = StreamsEnable.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StreamsEnable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Streams> GetPayload(HarpMessage message)
        {
            return StreamsEnable.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that disables the generation of events for the respective streams.
    /// </summary>
    [Description("Disables the generation of events for the respective streams.")]
    public partial class StreamsDisable
    {
        /// <summary>
        /// Represents the address of the <see cref="StreamsDisable"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="StreamsDisable"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="StreamsDisable"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StreamsDisable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Streams GetPayload(HarpMessage message)
        {
            return (Streams)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StreamsDisable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Streams> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Streams)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StreamsDisable"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StreamsDisable"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Streams value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StreamsDisable"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StreamsDisable"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Streams value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StreamsDisable register.
    /// </summary>
    /// <seealso cref="StreamsDisable"/>
    [Description("Filters and selects timestamped messages from the StreamsDisable register.")]
    public partial class TimestampedStreamsDisable
    {
        /// <summary>
        /// Represents the address of the <see cref="StreamsDisable"/> register. This field is constant.
        /// </summary>
        public const int Address = StreamsDisable.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StreamsDisable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Streams> GetPayload(HarpMessage message)
        {
            return StreamsDisable.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that stream containing data from Oximeter sensor reads.
    /// </summary>
    [Description("Stream containing data from Oximeter sensor reads.")]
    public partial class OximeterData
    {
        /// <summary>
        /// Represents the address of the <see cref="OximeterData"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="OximeterData"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OximeterData"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 4;

        static OximeterDataPayload ParsePayload(byte[] payload)
        {
            OximeterDataPayload result;
            result.Channel0 = payload[0];
            result.Channel1 = payload[1];
            result.Channel2 = payload[2];
            result.Channel3 = payload[3];
            return result;
        }

        static byte[] FormatPayload(OximeterDataPayload value)
        {
            byte[] result;
            result = new byte[4];
            result[0] = value.Channel0;
            result[1] = value.Channel1;
            result[2] = value.Channel2;
            result[3] = value.Channel3;
            return result;
        }

        /// <summary>
        /// Returns the payload data for <see cref="OximeterData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static OximeterDataPayload GetPayload(HarpMessage message)
        {
            return ParsePayload(message.GetPayloadArray<byte>());
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OximeterData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<OximeterDataPayload> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadArray<byte>();
            return Timestamped.Create(ParsePayload(payload.Value), payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OximeterData"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OximeterData"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, OximeterDataPayload value)
        {
            return HarpMessage.FromByte(Address, messageType, FormatPayload(value));
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OximeterData"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OximeterData"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, OximeterDataPayload value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, FormatPayload(value));
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OximeterData register.
    /// </summary>
    /// <seealso cref="OximeterData"/>
    [Description("Filters and selects timestamped messages from the OximeterData register.")]
    public partial class TimestampedOximeterData
    {
        /// <summary>
        /// Represents the address of the <see cref="OximeterData"/> register. This field is constant.
        /// </summary>
        public const int Address = OximeterData.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OximeterData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<OximeterDataPayload> GetPayload(HarpMessage message)
        {
            return OximeterData.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
    /// </summary>
    [Description("Stream containing synchronized data from Ecg and general-purpose ADC sensor reads.")]
    public partial class EcgAndAnalogData
    {
        /// <summary>
        /// Represents the address of the <see cref="EcgAndAnalogData"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="EcgAndAnalogData"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="EcgAndAnalogData"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 2;

        static EcgAndAnalogDataPayload ParsePayload(ushort[] payload)
        {
            EcgAndAnalogDataPayload result;
            result.Ecg = payload[0];
            result.Adc = payload[1];
            return result;
        }

        static ushort[] FormatPayload(EcgAndAnalogDataPayload value)
        {
            ushort[] result;
            result = new ushort[2];
            result[0] = value.Ecg;
            result[1] = value.Adc;
            return result;
        }

        /// <summary>
        /// Returns the payload data for <see cref="EcgAndAnalogData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EcgAndAnalogDataPayload GetPayload(HarpMessage message)
        {
            return ParsePayload(message.GetPayloadArray<ushort>());
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EcgAndAnalogData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EcgAndAnalogDataPayload> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadArray<ushort>();
            return Timestamped.Create(ParsePayload(payload.Value), payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EcgAndAnalogData"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EcgAndAnalogData"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EcgAndAnalogDataPayload value)
        {
            return HarpMessage.FromUInt16(Address, messageType, FormatPayload(value));
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EcgAndAnalogData"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EcgAndAnalogData"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EcgAndAnalogDataPayload value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, FormatPayload(value));
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EcgAndAnalogData register.
    /// </summary>
    /// <seealso cref="EcgAndAnalogData"/>
    [Description("Filters and selects timestamped messages from the EcgAndAnalogData register.")]
    public partial class TimestampedEcgAndAnalogData
    {
        /// <summary>
        /// Represents the address of the <see cref="EcgAndAnalogData"/> register. This field is constant.
        /// </summary>
        public const int Address = EcgAndAnalogData.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EcgAndAnalogData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EcgAndAnalogDataPayload> GetPayload(HarpMessage message)
        {
            return EcgAndAnalogData.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that stream containing data from Gsr sensor reads.
    /// </summary>
    [Description("Stream containing data from Gsr sensor reads.")]
    public partial class GsrData
    {
        /// <summary>
        /// Represents the address of the <see cref="GsrData"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="GsrData"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="GsrData"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="GsrData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="GsrData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="GsrData"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="GsrData"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="GsrData"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="GsrData"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// GsrData register.
    /// </summary>
    /// <seealso cref="GsrData"/>
    [Description("Filters and selects timestamped messages from the GsrData register.")]
    public partial class TimestampedGsrData
    {
        /// <summary>
        /// Represents the address of the <see cref="GsrData"/> register. This field is constant.
        /// </summary>
        public const int Address = GsrData.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="GsrData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return GsrData.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that stream that generates an event on each triggered acquisition by the external accelerometer board.
    /// </summary>
    [Description("Stream that generates an event on each triggered acquisition by the external accelerometer board.")]
    public partial class AccelerometerData
    {
        /// <summary>
        /// Represents the address of the <see cref="AccelerometerData"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="AccelerometerData"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="AccelerometerData"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AccelerometerData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AccelerometerData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AccelerometerData"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AccelerometerData"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AccelerometerData"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AccelerometerData"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AccelerometerData register.
    /// </summary>
    /// <seealso cref="AccelerometerData"/>
    [Description("Filters and selects timestamped messages from the AccelerometerData register.")]
    public partial class TimestampedAccelerometerData
    {
        /// <summary>
        /// Represents the address of the <see cref="AccelerometerData"/> register. This field is constant.
        /// </summary>
        public const int Address = AccelerometerData.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AccelerometerData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return AccelerometerData.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reflects the state of DI digital lines of each Port.
    /// </summary>
    [Description("Reflects the state of DI digital lines of each Port")]
    public partial class PortDigitalInput
    {
        /// <summary>
        /// Represents the address of the <see cref="PortDigitalInput"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="PortDigitalInput"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PortDigitalInput"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PortDigitalInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalInputs GetPayload(HarpMessage message)
        {
            return (DigitalInputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PortDigitalInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalInputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PortDigitalInput"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PortDigitalInput"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PortDigitalInput"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PortDigitalInput"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PortDigitalInput register.
    /// </summary>
    /// <seealso cref="PortDigitalInput"/>
    [Description("Filters and selects timestamped messages from the PortDigitalInput register.")]
    public partial class TimestampedPortDigitalInput
    {
        /// <summary>
        /// Represents the address of the <see cref="PortDigitalInput"/> register. This field is constant.
        /// </summary>
        public const int Address = PortDigitalInput.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PortDigitalInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetPayload(HarpMessage message)
        {
            return PortDigitalInput.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that set the specified digital output lines.
    /// </summary>
    [Description("Set the specified digital output lines.")]
    public partial class OutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="OutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OutputSet"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputSet"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OutputSet"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputSet"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OutputSet register.
    /// </summary>
    /// <seealso cref="OutputSet"/>
    [Description("Filters and selects timestamped messages from the OutputSet register.")]
    public partial class TimestampedOutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = OutputSet.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return OutputSet.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that clear the specified digital output lines.
    /// </summary>
    [Description("Clear the specified digital output lines")]
    public partial class OutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="OutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OutputClear"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputClear"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OutputClear"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputClear"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OutputClear register.
    /// </summary>
    /// <seealso cref="OutputClear"/>
    [Description("Filters and selects timestamped messages from the OutputClear register.")]
    public partial class TimestampedOutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = OutputClear.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return OutputClear.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// Pluma device.
    /// </summary>
    /// <seealso cref="CreateStreamsEnablePayload"/>
    /// <seealso cref="CreateStreamsDisablePayload"/>
    /// <seealso cref="CreateOximeterDataPayload"/>
    /// <seealso cref="CreateEcgAndAnalogDataPayload"/>
    /// <seealso cref="CreateGsrDataPayload"/>
    /// <seealso cref="CreateAccelerometerDataPayload"/>
    /// <seealso cref="CreatePortDigitalInputPayload"/>
    /// <seealso cref="CreateOutputSetPayload"/>
    /// <seealso cref="CreateOutputClearPayload"/>
    [XmlInclude(typeof(CreateStreamsEnablePayload))]
    [XmlInclude(typeof(CreateStreamsDisablePayload))]
    [XmlInclude(typeof(CreateOximeterDataPayload))]
    [XmlInclude(typeof(CreateEcgAndAnalogDataPayload))]
    [XmlInclude(typeof(CreateGsrDataPayload))]
    [XmlInclude(typeof(CreateAccelerometerDataPayload))]
    [XmlInclude(typeof(CreatePortDigitalInputPayload))]
    [XmlInclude(typeof(CreateOutputSetPayload))]
    [XmlInclude(typeof(CreateOutputClearPayload))]
    [XmlInclude(typeof(CreateTimestampedStreamsEnablePayload))]
    [XmlInclude(typeof(CreateTimestampedStreamsDisablePayload))]
    [XmlInclude(typeof(CreateTimestampedOximeterDataPayload))]
    [XmlInclude(typeof(CreateTimestampedEcgAndAnalogDataPayload))]
    [XmlInclude(typeof(CreateTimestampedGsrDataPayload))]
    [XmlInclude(typeof(CreateTimestampedAccelerometerDataPayload))]
    [XmlInclude(typeof(CreateTimestampedPortDigitalInputPayload))]
    [XmlInclude(typeof(CreateTimestampedOutputSetPayload))]
    [XmlInclude(typeof(CreateTimestampedOutputClearPayload))]
    [Description("Creates standard message payloads for the Pluma device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateStreamsEnablePayload();
        }

        string INamedElement.Name => $"{nameof(Pluma)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enables the generation of events for the respective streams.
    /// </summary>
    [DisplayName("StreamsEnablePayload")]
    [Description("Creates a message payload that enables the generation of events for the respective streams.")]
    public partial class CreateStreamsEnablePayload
    {
        /// <summary>
        /// Gets or sets the value that enables the generation of events for the respective streams.
        /// </summary>
        [Description("The value that enables the generation of events for the respective streams.")]
        public Streams StreamsEnable { get; set; }

        /// <summary>
        /// Creates a message payload for the StreamsEnable register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Streams GetPayload()
        {
            return StreamsEnable;
        }

        /// <summary>
        /// Creates a message that enables the generation of events for the respective streams.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StreamsEnable register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.StreamsEnable.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enables the generation of events for the respective streams.
    /// </summary>
    [DisplayName("TimestampedStreamsEnablePayload")]
    [Description("Creates a timestamped message payload that enables the generation of events for the respective streams.")]
    public partial class CreateTimestampedStreamsEnablePayload : CreateStreamsEnablePayload
    {
        /// <summary>
        /// Creates a timestamped message that enables the generation of events for the respective streams.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StreamsEnable register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.StreamsEnable.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that disables the generation of events for the respective streams.
    /// </summary>
    [DisplayName("StreamsDisablePayload")]
    [Description("Creates a message payload that disables the generation of events for the respective streams.")]
    public partial class CreateStreamsDisablePayload
    {
        /// <summary>
        /// Gets or sets the value that disables the generation of events for the respective streams.
        /// </summary>
        [Description("The value that disables the generation of events for the respective streams.")]
        public Streams StreamsDisable { get; set; }

        /// <summary>
        /// Creates a message payload for the StreamsDisable register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Streams GetPayload()
        {
            return StreamsDisable;
        }

        /// <summary>
        /// Creates a message that disables the generation of events for the respective streams.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StreamsDisable register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.StreamsDisable.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that disables the generation of events for the respective streams.
    /// </summary>
    [DisplayName("TimestampedStreamsDisablePayload")]
    [Description("Creates a timestamped message payload that disables the generation of events for the respective streams.")]
    public partial class CreateTimestampedStreamsDisablePayload : CreateStreamsDisablePayload
    {
        /// <summary>
        /// Creates a timestamped message that disables the generation of events for the respective streams.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StreamsDisable register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.StreamsDisable.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that stream containing data from Oximeter sensor reads.
    /// </summary>
    [DisplayName("OximeterDataPayload")]
    [Description("Creates a message payload that stream containing data from Oximeter sensor reads.")]
    public partial class CreateOximeterDataPayload
    {
        /// <summary>
        /// Gets or sets a value to write on payload member Channel0.
        /// </summary>
        [Description("")]
        public byte Channel0 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel1.
        /// </summary>
        [Description("")]
        public byte Channel1 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel2.
        /// </summary>
        [Description("")]
        public byte Channel2 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel3.
        /// </summary>
        [Description("")]
        public byte Channel3 { get; set; }

        /// <summary>
        /// Creates a message payload for the OximeterData register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public OximeterDataPayload GetPayload()
        {
            OximeterDataPayload value;
            value.Channel0 = Channel0;
            value.Channel1 = Channel1;
            value.Channel2 = Channel2;
            value.Channel3 = Channel3;
            return value;
        }

        /// <summary>
        /// Creates a message that stream containing data from Oximeter sensor reads.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OximeterData register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.OximeterData.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that stream containing data from Oximeter sensor reads.
    /// </summary>
    [DisplayName("TimestampedOximeterDataPayload")]
    [Description("Creates a timestamped message payload that stream containing data from Oximeter sensor reads.")]
    public partial class CreateTimestampedOximeterDataPayload : CreateOximeterDataPayload
    {
        /// <summary>
        /// Creates a timestamped message that stream containing data from Oximeter sensor reads.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OximeterData register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.OximeterData.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
    /// </summary>
    [DisplayName("EcgAndAnalogDataPayload")]
    [Description("Creates a message payload that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.")]
    public partial class CreateEcgAndAnalogDataPayload
    {
        /// <summary>
        /// Gets or sets a value to write on payload member Ecg.
        /// </summary>
        [Description("")]
        public ushort Ecg { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Adc.
        /// </summary>
        [Description("")]
        public ushort Adc { get; set; }

        /// <summary>
        /// Creates a message payload for the EcgAndAnalogData register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EcgAndAnalogDataPayload GetPayload()
        {
            EcgAndAnalogDataPayload value;
            value.Ecg = Ecg;
            value.Adc = Adc;
            return value;
        }

        /// <summary>
        /// Creates a message that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EcgAndAnalogData register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.EcgAndAnalogData.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
    /// </summary>
    [DisplayName("TimestampedEcgAndAnalogDataPayload")]
    [Description("Creates a timestamped message payload that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.")]
    public partial class CreateTimestampedEcgAndAnalogDataPayload : CreateEcgAndAnalogDataPayload
    {
        /// <summary>
        /// Creates a timestamped message that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EcgAndAnalogData register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.EcgAndAnalogData.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that stream containing data from Gsr sensor reads.
    /// </summary>
    [DisplayName("GsrDataPayload")]
    [Description("Creates a message payload that stream containing data from Gsr sensor reads.")]
    public partial class CreateGsrDataPayload
    {
        /// <summary>
        /// Gets or sets the value that stream containing data from Gsr sensor reads.
        /// </summary>
        [Description("The value that stream containing data from Gsr sensor reads.")]
        public ushort GsrData { get; set; }

        /// <summary>
        /// Creates a message payload for the GsrData register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return GsrData;
        }

        /// <summary>
        /// Creates a message that stream containing data from Gsr sensor reads.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the GsrData register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.GsrData.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that stream containing data from Gsr sensor reads.
    /// </summary>
    [DisplayName("TimestampedGsrDataPayload")]
    [Description("Creates a timestamped message payload that stream containing data from Gsr sensor reads.")]
    public partial class CreateTimestampedGsrDataPayload : CreateGsrDataPayload
    {
        /// <summary>
        /// Creates a timestamped message that stream containing data from Gsr sensor reads.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the GsrData register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.GsrData.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that stream that generates an event on each triggered acquisition by the external accelerometer board.
    /// </summary>
    [DisplayName("AccelerometerDataPayload")]
    [Description("Creates a message payload that stream that generates an event on each triggered acquisition by the external accelerometer board.")]
    public partial class CreateAccelerometerDataPayload
    {
        /// <summary>
        /// Gets or sets the value that stream that generates an event on each triggered acquisition by the external accelerometer board.
        /// </summary>
        [Description("The value that stream that generates an event on each triggered acquisition by the external accelerometer board.")]
        public byte AccelerometerData { get; set; }

        /// <summary>
        /// Creates a message payload for the AccelerometerData register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return AccelerometerData;
        }

        /// <summary>
        /// Creates a message that stream that generates an event on each triggered acquisition by the external accelerometer board.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AccelerometerData register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.AccelerometerData.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that stream that generates an event on each triggered acquisition by the external accelerometer board.
    /// </summary>
    [DisplayName("TimestampedAccelerometerDataPayload")]
    [Description("Creates a timestamped message payload that stream that generates an event on each triggered acquisition by the external accelerometer board.")]
    public partial class CreateTimestampedAccelerometerDataPayload : CreateAccelerometerDataPayload
    {
        /// <summary>
        /// Creates a timestamped message that stream that generates an event on each triggered acquisition by the external accelerometer board.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AccelerometerData register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.AccelerometerData.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that reflects the state of DI digital lines of each Port.
    /// </summary>
    [DisplayName("PortDigitalInputPayload")]
    [Description("Creates a message payload that reflects the state of DI digital lines of each Port.")]
    public partial class CreatePortDigitalInputPayload
    {
        /// <summary>
        /// Gets or sets the value that reflects the state of DI digital lines of each Port.
        /// </summary>
        [Description("The value that reflects the state of DI digital lines of each Port.")]
        public DigitalInputs PortDigitalInput { get; set; }

        /// <summary>
        /// Creates a message payload for the PortDigitalInput register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalInputs GetPayload()
        {
            return PortDigitalInput;
        }

        /// <summary>
        /// Creates a message that reflects the state of DI digital lines of each Port.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PortDigitalInput register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.PortDigitalInput.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that reflects the state of DI digital lines of each Port.
    /// </summary>
    [DisplayName("TimestampedPortDigitalInputPayload")]
    [Description("Creates a timestamped message payload that reflects the state of DI digital lines of each Port.")]
    public partial class CreateTimestampedPortDigitalInputPayload : CreatePortDigitalInputPayload
    {
        /// <summary>
        /// Creates a timestamped message that reflects the state of DI digital lines of each Port.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PortDigitalInput register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.PortDigitalInput.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("OutputSetPayload")]
    [Description("Creates a message payload that set the specified digital output lines.")]
    public partial class CreateOutputSetPayload
    {
        /// <summary>
        /// Gets or sets the value that set the specified digital output lines.
        /// </summary>
        [Description("The value that set the specified digital output lines.")]
        public DigitalOutputs OutputSet { get; set; }

        /// <summary>
        /// Creates a message payload for the OutputSet register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return OutputSet;
        }

        /// <summary>
        /// Creates a message that set the specified digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OutputSet register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.OutputSet.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputSetPayload")]
    [Description("Creates a timestamped message payload that set the specified digital output lines.")]
    public partial class CreateTimestampedOutputSetPayload : CreateOutputSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that set the specified digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OutputSet register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.OutputSet.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("OutputClearPayload")]
    [Description("Creates a message payload that clear the specified digital output lines.")]
    public partial class CreateOutputClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clear the specified digital output lines.
        /// </summary>
        [Description("The value that clear the specified digital output lines.")]
        public DigitalOutputs OutputClear { get; set; }

        /// <summary>
        /// Creates a message payload for the OutputClear register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return OutputClear;
        }

        /// <summary>
        /// Creates a message that clear the specified digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OutputClear register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return EmotionalCities.Pluma.OutputClear.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputClearPayload")]
    [Description("Creates a timestamped message payload that clear the specified digital output lines.")]
    public partial class CreateTimestampedOutputClearPayload : CreateOutputClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clear the specified digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OutputClear register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return EmotionalCities.Pluma.OutputClear.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents the payload of the OximeterData register.
    /// </summary>
    public struct OximeterDataPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OximeterDataPayload"/> structure.
        /// </summary>
        /// <param name="channel0"></param>
        /// <param name="channel1"></param>
        /// <param name="channel2"></param>
        /// <param name="channel3"></param>
        public OximeterDataPayload(
            byte channel0,
            byte channel1,
            byte channel2,
            byte channel3)
        {
            Channel0 = channel0;
            Channel1 = channel1;
            Channel2 = channel2;
            Channel3 = channel3;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Channel0;

        /// <summary>
        /// 
        /// </summary>
        public byte Channel1;

        /// <summary>
        /// 
        /// </summary>
        public byte Channel2;

        /// <summary>
        /// 
        /// </summary>
        public byte Channel3;
    }

    /// <summary>
    /// Represents the payload of the EcgAndAnalogData register.
    /// </summary>
    public struct EcgAndAnalogDataPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EcgAndAnalogDataPayload"/> structure.
        /// </summary>
        /// <param name="ecg"></param>
        /// <param name="adc"></param>
        public EcgAndAnalogDataPayload(
            ushort ecg,
            ushort adc)
        {
            Ecg = ecg;
            Adc = adc;
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort Ecg;

        /// <summary>
        /// 
        /// </summary>
        public ushort Adc;
    }

    /// <summary>
    /// Avaialble streams in the device.
    /// </summary>
    [Flags]
    public enum Streams : byte
    {
        None = 0x0,
        Oximeter = 0x1,
        Ecg = 0x2,
        Gsr = 0x4,
        Accelerometer = 0x8
    }

    /// <summary>
    /// Specifies the state of port digital input lines.
    /// </summary>
    [Flags]
    public enum DigitalInputs : byte
    {
        None = 0x0,
        DI0 = 0x1,
        DI1 = 0x2
    }

    /// <summary>
    /// Specifies the state of port digital output lines.
    /// </summary>
    [Flags]
    public enum DigitalOutputs : byte
    {
        None = 0x0,
        DO0 = 0x1,
        DO1 = 0x2
    }
}
