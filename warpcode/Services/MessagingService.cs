using CommunityToolkit.Mvvm.Messaging;
using System;

namespace CCIMIGRATION.Services
{
    /// <summary>
    /// Service wrapper for CommunityToolkit.Mvvm.Messaging to replace obsolete MessagingCenter
    /// </summary>
    public static class MessagingService
    {
        private static readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

        /// <summary>
        /// Send a message to all subscribers (for non-string, non-enum types)
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">The message to send</param>
        /// <param name="token">Optional token for message filtering</param>
        public static void SendMessage<T>(T message, string token = null) where T : class
        {
            if (!string.IsNullOrEmpty(token))
            {
                _messenger.Send(message, token);
            }
            else
            {
                _messenger.Send(message);
            }
        }

        /// <summary>
        /// Send a simple string message (common in the existing codebase)
        /// </summary>
        /// <param name="message">The message string</param>
        /// <param name="token">Message token/key</param>
        public static void Send(string message, string token)
        {
            _messenger.Send(new StringMessage(message), token);
        }

        /// <summary>
        /// Send an enum message (for TransitionType and similar enums)
        /// </summary>
        /// <param name="enumValue">The enum value to send</param>
        /// <param name="token">Message token/key</param>
        public static void SendEnum<T>(T enumValue, string token) where T : struct, System.Enum
        {
            _messenger.Send(new EnumMessage<T>(enumValue), token);
        }

        /// <summary>
        /// Subscribe to messages of a specific type (for non-string, non-enum types)
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="recipient">The subscriber</param>
        /// <param name="handler">Message handler</param>
        /// <param name="token">Optional token for message filtering</param>
        public static void SubscribeMessage<T>(object recipient, Action<T> handler, string token = null) where T : class
        {
            if (!string.IsNullOrEmpty(token))
            {
                _messenger.Register<T, string>(recipient, token, (r, m) => handler(m));
            }
            else
            {
                _messenger.Register<T>(recipient, (r, m) => handler(m));
            }
        }

        /// <summary>
        /// Subscribe to string messages with a specific token (backward compatibility)
        /// </summary>
        /// <param name="recipient">The subscriber</param>
        /// <param name="token">Message token/key</param>
        /// <param name="handler">Message handler</param>
        public static void Subscribe(object recipient, string token, Action<string> handler)
        {
            _messenger.Register<StringMessage, string>(recipient, token, (r, m) => handler(m.Value));
        }

        /// <summary>
        /// Subscribe to enum messages with a specific token (for TransitionType etc.)
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="recipient">The subscriber</param>
        /// <param name="token">Message token/key</param>
        /// <param name="handler">Message handler</param>
        public static void SubscribeEnum<T>(object recipient, string token, Action<T> handler) where T : struct, System.Enum
        {
            _messenger.Register<EnumMessage<T>, string>(recipient, token, (r, m) => handler(m.Value));
        }

        /// <summary>
        /// Unsubscribe from all messages for a recipient
        /// </summary>
        /// <param name="recipient">The subscriber to unsubscribe</param>
        public static void Unsubscribe(object recipient)
        {
            _messenger.UnregisterAll(recipient);
        }

        /// <summary>
        /// Unsubscribe from messages of a specific type
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="recipient">The subscriber</param>
        /// <param name="token">Optional token</param>
        public static void Unsubscribe<T>(object recipient, string token = null) where T : class
        {
            if (!string.IsNullOrEmpty(token))
            {
                _messenger.Unregister<T, string>(recipient, token);
            }
            else
            {
                _messenger.Unregister<T>(recipient);
            }
        }

        /// <summary>
        /// Unsubscribe from string messages with specific token
        /// </summary>
        /// <param name="recipient">The subscriber</param>
        /// <param name="token">Message token</param>
        public static void Unsubscribe(object recipient, string token)
        {
            _messenger.Unregister<StringMessage, string>(recipient, token);
        }
    }

    /// <summary>
    /// Simple string message wrapper for backward compatibility
    /// </summary>
    public class StringMessage
    {
        public string Value { get; }

        public StringMessage(string value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Enum message wrapper for backward compatibility
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    public class EnumMessage<T> where T : struct, System.Enum
    {
        public T Value { get; }

        public EnumMessage(T value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Generic message with sender and message for complex scenarios
    /// </summary>
    /// <typeparam name="TSender">Sender type</typeparam>
    /// <typeparam name="TMessage">Message type</typeparam>
    public class SenderMessage<TSender, TMessage>
    {
        public TSender Sender { get; }
        public TMessage Message { get; }

        public SenderMessage(TSender sender, TMessage message)
        {
            Sender = sender;
            Message = message;
        }
    }
}
