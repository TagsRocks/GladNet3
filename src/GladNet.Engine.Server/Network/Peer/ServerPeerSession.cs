﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet.Engine.Common;
using GladNet.Message;

namespace GladNet.Engine.Server
{
	/// <summary>
	/// Session for remote subserver peers that has the same functionality as <see cref="ClientPeerSession"/>s.
	/// </summary>
	public abstract class ServerPeerSession : ClientPeerSession
	{
		public ServerPeerSession(ILog logger, INetworkMessageRouterService sender, IConnectionDetails details, INetworkMessageSubscriptionService netMessageSubService,
			IDisconnectionServiceHandler disconnectHandler, INetworkMessageRouteBackService routebackService)
				: base(logger, sender, details, netMessageSubService, disconnectHandler, routebackService)
		{

		}

		/// <summary>
		/// Called internally when a request is recieved from the remote peer.
		/// </summary>
		/// <param name="requestMessage"><see cref="IRequestMessage"/> sent by the peer.</param>
		/// <param name="parameters">Parameters the message was sent with.</param>
		protected override abstract void OnReceiveRequest(IRequestMessage requestMessage, IMessageParameters parameters);
	}
}