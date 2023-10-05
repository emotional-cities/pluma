using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.Pluma
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
    public class FilterMessage : FilterMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterMessage"/> class.
        /// </summary>
        public FilterMessage()
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
    /// Represents an operator that creates a sequence of message payloads
    /// that enables the generation of events for the respective streams.
    /// </summary>
    [DisplayName("StreamsEnablePayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that enables the generation of events for the respective streams.")]
    public partial class CreateStreamsEnablePayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that enables the generation of events for the respective streams.
        /// </summary>
        [Description("The value that enables the generation of events for the respective streams.")]
        public Streams Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that enables the generation of events for the respective streams.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that enables the generation of events for the respective streams.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => StreamsEnable.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that disables the generation of events for the respective streams.
    /// </summary>
    [DisplayName("StreamsDisablePayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that disables the generation of events for the respective streams.")]
    public partial class CreateStreamsDisablePayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that disables the generation of events for the respective streams.
        /// </summary>
        [Description("The value that disables the generation of events for the respective streams.")]
        public Streams Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that disables the generation of events for the respective streams.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that disables the generation of events for the respective streams.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => StreamsDisable.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that stream containing data from Oximeter sensor reads.
    /// </summary>
    [DisplayName("OximeterDataPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that stream containing data from Oximeter sensor reads.")]
    public partial class CreateOximeterDataPayload : HarpCombinator
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
        /// Creates an observable sequence that contains a single message
        /// that stream containing data from Oximeter sensor reads.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that stream containing data from Oximeter sensor reads.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ =>
            {
                OximeterDataPayload value;
                value.Channel0 = Channel0;
                value.Channel1 = Channel1;
                value.Channel2 = Channel2;
                value.Channel3 = Channel3;
                return OximeterData.FromPayload(MessageType, value);
            });
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
    /// </summary>
    [DisplayName("EcgAndAnalogDataPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.")]
    public partial class CreateEcgAndAnalogDataPayload : HarpCombinator
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
        /// Creates an observable sequence that contains a single message
        /// that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that stream containing synchronized data from Ecg and general-purpose ADC sensor reads.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ =>
            {
                EcgAndAnalogDataPayload value;
                value.Ecg = Ecg;
                value.Adc = Adc;
                return EcgAndAnalogData.FromPayload(MessageType, value);
            });
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that stream containing data from Gsr sensor reads.
    /// </summary>
    [DisplayName("GsrDataPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that stream containing data from Gsr sensor reads.")]
    public partial class CreateGsrDataPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that stream containing data from Gsr sensor reads.
        /// </summary>
        [Description("The value that stream containing data from Gsr sensor reads.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that stream containing data from Gsr sensor reads.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that stream containing data from Gsr sensor reads.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => GsrData.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that stream that generates an event on each triggered acquisition by the external accelerometer board.
    /// </summary>
    [DisplayName("AccelerometerDataPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that stream that generates an event on each triggered acquisition by the external accelerometer board.")]
    public partial class CreateAccelerometerDataPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that stream that generates an event on each triggered acquisition by the external accelerometer board.
        /// </summary>
        [Description("The value that stream that generates an event on each triggered acquisition by the external accelerometer board.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that stream that generates an event on each triggered acquisition by the external accelerometer board.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that stream that generates an event on each triggered acquisition by the external accelerometer board.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AccelerometerData.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that reflects the state of DI digital lines of each Port.
    /// </summary>
    [DisplayName("PortDigitalInputPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that reflects the state of DI digital lines of each Port.")]
    public partial class CreatePortDigitalInputPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that reflects the state of DI digital lines of each Port.
        /// </summary>
        [Description("The value that reflects the state of DI digital lines of each Port.")]
        public DigitalInputs Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that reflects the state of DI digital lines of each Port.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that reflects the state of DI digital lines of each Port.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => PortDigitalInput.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("OutputSetPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that set the specified digital output lines.")]
    public partial class CreateOutputSetPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that set the specified digital output lines.
        /// </summary>
        [Description("The value that set the specified digital output lines.")]
        public DigitalOutputs Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that set the specified digital output lines.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that set the specified digital output lines.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => OutputSet.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("OutputClearPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that clear the specified digital output lines.")]
    public partial class CreateOutputClearPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that clear the specified digital output lines.
        /// </summary>
        [Description("The value that clear the specified digital output lines.")]
        public DigitalOutputs Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that clear the specified digital output lines.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that clear the specified digital output lines.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => OutputClear.FromPayload(MessageType, Value));
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
        DI0 = 0x1,
        DI1 = 0x2
    }

    /// <summary>
    /// Specifies the state of port digital output lines.
    /// </summary>
    [Flags]
    public enum DigitalOutputs : byte
    {
        DO0 = 0x1,
        DO1 = 0x2
    }
}
