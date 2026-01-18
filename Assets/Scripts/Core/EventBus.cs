using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidFoundry.Core
{
    /// <summary>
    /// Global event bus for decoupled communication between systems
    /// Uses publish/subscribe pattern
    /// </summary>
    public static class EventBus
    {
        #region Subscription Management

        // Dictionary: EventType -> List of handlers
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        // Track subscriptions for debugging
        private static readonly Dictionary<Type, int> _subscriptionCounts = new Dictionary<Type, int>();

        #endregion

        #region Subscribe

        /// <summary>
        /// Subscribe to an event type
        /// </summary>
        /// <typeparam name="T">Event type to subscribe to</typeparam>
        /// <param name="handler">Handler to call when event is published</param>
        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            Type eventType = typeof(T);

            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
                _subscriptionCounts[eventType] = 0;
            }

            // Check for duplicate subscriptions
            if (_subscribers[eventType].Contains(handler))
            {
                Debug.LogWarning($"[EventBus] Duplicate subscription attempt for {eventType.Name}");
                return;
            }

            _subscribers[eventType].Add(handler);
            _subscriptionCounts[eventType]++;

#if UNITY_EDITOR && DEBUG_EVENTBUS
            Debug.Log($"[EventBus] Subscribed to {eventType.Name}. Total subscribers: {_subscriptionCounts[eventType]}");
#endif
        }

        #endregion

        #region Unsubscribe

        /// <summary>
        /// Unsubscribe from an event type
        /// </summary>
        /// <typeparam name="T">Event type to unsubscribe from</typeparam>
        /// <param name="handler">Handler to remove</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            Type eventType = typeof(T);

            if (!_subscribers.ContainsKey(eventType))
            {
                Debug.LogWarning($"[EventBus] Attempted to unsubscribe from {eventType.Name} but no subscriptions exist");
                return;
            }

            if (_subscribers[eventType].Remove(handler))
            {
                _subscriptionCounts[eventType]--;

#if UNITY_EDITOR && DEBUG_EVENTBUS
                Debug.Log($"[EventBus] Unsubscribed from {eventType.Name}. Remaining subscribers: {_subscriptionCounts[eventType]}");
#endif

                // Clean up empty lists
                if (_subscribers[eventType].Count == 0)
                {
                    _subscribers.Remove(eventType);
                    _subscriptionCounts.Remove(eventType);
                }
            }
            else
            {
                Debug.LogWarning($"[EventBus] Handler not found for {eventType.Name}");
            }
        }

        #endregion

        #region Publish

        /// <summary>
        /// Publish an event to all subscribers
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="eventData">Event data to send</param>
        public static void Publish<T>(T eventData) where T : struct
        {
            Type eventType = typeof(T);

            if (!_subscribers.ContainsKey(eventType))
            {
#if UNITY_EDITOR && DEBUG_EVENTBUS
                Debug.Log($"[EventBus] Published {eventType.Name} but no subscribers");
#endif
                return;
            }

#if UNITY_EDITOR && DEBUG_EVENTBUS
            Debug.Log($"[EventBus] Publishing {eventType.Name} to {_subscribers[eventType].Count} subscribers");
#endif

            // Create a copy of the list to avoid modification during iteration
            List<Delegate> handlers = new List<Delegate>(_subscribers[eventType]);

            foreach (Delegate handler in handlers)
            {
                try
                {
                    ((Action<T>)handler)?.Invoke(eventData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[EventBus] Error invoking handler for {eventType.Name}: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// Clear all subscriptions (use with caution, mainly for testing)
        /// </summary>
        public static void ClearAll()
        {
            int totalSubscribers = 0;
            foreach (var count in _subscriptionCounts.Values)
            {
                totalSubscribers += count;
            }

            _subscribers.Clear();
            _subscriptionCounts.Clear();

            Debug.LogWarning($"[EventBus] Cleared all subscriptions ({totalSubscribers} total)");
        }

        /// <summary>
        /// Clear subscriptions for a specific event type
        /// </summary>
        public static void Clear<T>() where T : struct
        {
            Type eventType = typeof(T);

            if (_subscribers.ContainsKey(eventType))
            {
                int count = _subscriptionCounts[eventType];
                _subscribers.Remove(eventType);
                _subscriptionCounts.Remove(eventType);

                Debug.LogWarning($"[EventBus] Cleared {count} subscriptions for {eventType.Name}");
            }
        }

        /// <summary>
        /// Get number of subscribers for an event type
        /// </summary>
        public static int GetSubscriberCount<T>() where T : struct
        {
            Type eventType = typeof(T);
            return _subscriptionCounts.ContainsKey(eventType) ? _subscriptionCounts[eventType] : 0;
        }

        /// <summary>
        /// Check if there are any subscribers for an event type
        /// </summary>
        public static bool HasSubscribers<T>() where T : struct
        {
            Type eventType = typeof(T);
            return _subscribers.ContainsKey(eventType) && _subscribers[eventType].Count > 0;
        }

        #endregion

        #region Debug

#if UNITY_EDITOR
        /// <summary>
        /// Print all current subscriptions (debug only)
        /// </summary>
        public static void DebugPrintSubscriptions()
        {
            Debug.Log("=== EventBus Subscriptions ===");

            if (_subscriptionCounts.Count == 0)
            {
                Debug.Log("No active subscriptions");
                return;
            }

            foreach (var kvp in _subscriptionCounts)
            {
                Debug.Log($"{kvp.Key.Name}: {kvp.Value} subscriber(s)");
            }

            Debug.Log("=============================");
        }
#endif

        #endregion
    }

    #region Example Event Definitions

    // These are example event structures - actual events will be defined in their respective systems

    /// <summary>
    /// Example: Game state changed event
    /// </summary>
    public struct GameStateChangedEvent
    {
        public GameManager.GameState newState;
        public GameManager.GameState previousState;
    }

    /// <summary>
    /// Example: Game paused event
    /// </summary>
    public struct GamePausedEvent
    {
        public float gameTime;
    }

    /// <summary>
    /// Example: Game resumed event
    /// </summary>
    public struct GameResumedEvent
    {
        public float gameTime;
    }

    /// <summary>
    /// Example: Time scale changed event
    /// </summary>
    public struct TimeScaleChangedEvent
    {
        public float newScale;
        public float previousScale;
    }

    // More events will be defined as we implement systems:
    // - DroneSpawnedEvent
    // - DroneDestroyedEvent
    // - ResourceMinedEvent
    // - StructureBuiltEvent
    // - MissionCompletedEvent
    // etc.

    #endregion
}
