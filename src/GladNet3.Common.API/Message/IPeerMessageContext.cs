﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GladNet;

namespace GladNet
{
	/// <summary>
	/// Contract for the context of a message.
	/// </summary>
	public interface IPeerMessageContext<in TPayloadBaseType>
		where TPayloadBaseType : class
	{
		//Below you'll see a bunch of interfaces that the client
		//actually implements. However we don't have to generic to hell
		//all of the code. It becomes too cumbersome to understand and consume in that
		//case and leads to generic type parameter carrying, a design fault.

		/// <summary>
		/// The connection service that provides ways to disconnect and connect
		/// the client associated with this message conect.
		/// </summary>
		IConnectionService ConnectionService { get; }

		/// <summary>
		/// The sending service that allows clients to send
		/// a response or request to the client context involved in this particular message.
		/// </summary>
		IPeerPayloadSendService<TPayloadBaseType> PayloadSendService { get; }

		//TODO: What should we do for servers? They can't do async/intercepts
		/// <summary>
		/// The request service that registers a response and returns a task
		/// that can be awaited on for when/if the response is recieved.
		/// </summary>
		[Obsolete("This will be removed eventually and put into a different Client interface.")]
		IPeerRequestSendService<TPayloadBaseType> RequestSendService { get; }
	}
}
