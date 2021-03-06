﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladNet
{

	/// <summary>
	/// Handler that implements try semantics in attempting to handle a provided message.
	/// It can indicate if the message is consumed/consumable.
	/// </summary>
	/// <typeparam name="TIncomingPayloadType"></typeparam>
	/// <typeparam name="TOutgoingPayloadType"></typeparam>
	/// <typeparam name="TPayloadType"></typeparam>
	public sealed class TrySemanticsBasedOnTypePeerMessageHandler<TIncomingPayloadType, TOutgoingPayloadType, TPayloadType> : TrySemanticsBasedOnTypePeerMessageHandler<TIncomingPayloadType, TOutgoingPayloadType, TPayloadType, IPeerMessageContext<TOutgoingPayloadType>>, IPeerMessageHandler<TIncomingPayloadType, TOutgoingPayloadType>
		where TIncomingPayloadType : class
		where TPayloadType : class, TIncomingPayloadType
		where TOutgoingPayloadType : class
	{
		/// <inheritdoc />
		public TrySemanticsBasedOnTypePeerMessageHandler(IPeerPayloadSpecificMessageHandler<TPayloadType, TOutgoingPayloadType, IPeerMessageContext<TOutgoingPayloadType>> decoratedPayloadHandler) 
			: base(decoratedPayloadHandler)
		{

		}
	}

	/// <summary>
	/// Handler that implements try semantics in attempting to handle a provided message.
	/// It can indicate if the message is consumed/consumable.
	/// </summary>
	/// <typeparam name="TIncomingPayloadType"></typeparam>
	/// <typeparam name="TOutgoingPayloadType"></typeparam>
	/// <typeparam name="TPayloadType"></typeparam>
	/// <typeparam name="TPeerContextType"></typeparam>
	public class TrySemanticsBasedOnTypePeerMessageHandler<TIncomingPayloadType, TOutgoingPayloadType, TPayloadType, TPeerContextType> : IPeerMessageHandler<TIncomingPayloadType, TOutgoingPayloadType, TPeerContextType>
		where TIncomingPayloadType : class
		where TPayloadType : class, TIncomingPayloadType
		where TOutgoingPayloadType : class
		where TPeerContextType : IPeerMessageContext<TOutgoingPayloadType>
	{
		/// <summary>
		/// Decorated payload handler that can handle
		/// payloads of type <typeparamref name="TPayloadType"/>.
		/// </summary>
		private IPeerPayloadSpecificMessageHandler<TPayloadType, TOutgoingPayloadType, TPeerContextType> DecoratedPayloadHandler { get; }

		/// <inheritdoc />
		public TrySemanticsBasedOnTypePeerMessageHandler(IPeerPayloadSpecificMessageHandler<TPayloadType, TOutgoingPayloadType, TPeerContextType> decoratedPayloadHandler)
		{
			if(decoratedPayloadHandler == null) throw new ArgumentNullException(nameof(decoratedPayloadHandler));

			DecoratedPayloadHandler = decoratedPayloadHandler;
		}

		/// <inheritdoc />
		public bool CanHandle(NetworkIncomingMessage<TIncomingPayloadType> message)
		{
			//We can handle a message if it's the payload type
			return message?.Payload is TPayloadType;
		}

		/// <summary>
		/// Attempts to handle the provided <see cref="message"/> and will succeed if the
		/// payload is of type <typeparamref name="TPayloadType"/>.
		/// Otherwise will return false and not consume the message.
		/// </summary>
		/// <param name="context">The context of the message.</param>
		/// <param name="message">The message.</param>
		/// <returns>True if the message has been consumed.</returns>
		public async Task<bool> TryHandleMessage(TPeerContextType context, NetworkIncomingMessage<TIncomingPayloadType> message)
		{
			if(context == null) throw new ArgumentNullException(nameof(context));
			if(message == null) throw new ArgumentNullException(nameof(message));

			if(message.Payload is TPayloadType payload)
			{
				//TODO: Should we not configureawait false here? Should we want to rejoin the context?
				//No matter what happens in the handler we should indicate that it's consumed
				await DecoratedPayloadHandler.HandleMessage(context, payload)
					.ConfigureAwait(false);

				return true;
			}

			//Default semantics is a handler can only handle a specific type
			//So we just indicate that we can't handle the message and the caller
			//will hopefully find someone else to handle it.
			return false;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.DecoratedPayloadHandler.ToString();
		}
	}
}
