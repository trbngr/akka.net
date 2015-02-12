﻿using System;
using System.Linq;
namespace Akka.Actor
{
    /// <summary>
    ///     Class ActorBase.
    /// </summary>
    public abstract partial class ActorBase
    {
        /// <summary>
        ///     Can be overridden to intercept calls to `preRestart`. Calls `preRestart` by default.
        /// </summary>
        /// <param name="cause">The cause.</param>
        /// <param name="message">The message.</param>
        public virtual void AroundPreRestart(Exception cause, object message)
        {
            PreRestart(cause, message);
        }

        /// <summary>
        ///     Can be overridden to intercept calls to `preStart`. Calls `preStart` by default.
        /// </summary>
        public virtual void AroundPreStart()
        {
            PreStart();
        }

        /// <summary>
        ///     User overridable callback.
        ///     <p />
        ///     Is called when an Actor is started.
        ///     Actors are automatically started asynchronously when created.
        ///     Empty default implementation.
        /// </summary>
        protected virtual void PreStart()
        {
        }

        /// <summary>
        ///     Can be overridden to intercept calls to `postRestart`. Calls `postRestart` by default.
        /// </summary>
        /// <param name="cause">The cause.</param>
        /// <param name="message">The message.</param>
        public virtual void AroundPostRestart(Exception cause, object message)
        {
            PostRestart(cause);
        }

        /// <summary>
        ///     User overridable callback: '''By default it disposes of all children and then calls `postStop()`.'''
        ///     <p />
        ///     Is called on a crashed Actor right BEFORE it is restarted to allow clean
        ///     up of resources before Actor is terminated.
        /// </summary>
        /// <param name="reason">the Exception that caused the restart to happen.</param>
        /// <param name="message">optionally the current message the actor processed when failing, if applicable.</param>
        protected virtual void PreRestart(Exception reason, object message)
        {
            Context.GetChildren().ToList().ForEach(c =>
            {
                Context.Unwatch(c);
                Context.Stop(c);
            });
            PostStop();
        }

        /// <summary>
        ///     User overridable callback: By default it calls `preStart()`.
        ///     <p />
        ///     Is called right AFTER restart on the newly created Actor to allow reinitialization after an Actor crash.
        /// </summary>
        /// <param name="reason">the Exception that caused the restart to happen.</param>
        protected virtual void PostRestart(Exception reason)
        {
            PreStart();
        }

        /// <summary>
        ///     Can be overridden to intercept calls to `postStop`. Calls `postStop` by default..
        /// </summary>
        public virtual void AroundPostStop()
        {
            PostStop();
        }

        /// <summary>
        ///     User overridable callback.
        ///     <p />
        ///     Is called asynchronously after 'actor.stop()' is invoked.
        ///     Empty default implementation.
        /// </summary>
        protected virtual void PostStop()
        {
        }
    }
}