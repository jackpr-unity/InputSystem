using System;

namespace ISX
{
    // Pointer to an InputEvent. Makes it easier to work with InputEvents and hides
    // the unsafe operations necessary to work with events.
    public unsafe struct InputEventPtr
    {
        // C# does not allow us to have pointers to structs that have managed data members. Since
        // this can't be guaranteed for generic type parameters, they can't be used with pointers.
        // This is why we cannot make InputEventPtr generic or have a generic method that returns
        // a pointer to a specific type of event.
        private InputEvent* m_EventPtr;

        public InputEventPtr(InputEvent* eventPtr)
        {
            m_EventPtr = eventPtr;
        }

        public bool valid => m_EventPtr != null;

        public bool handled
        {
            get
            {
                if (!valid)
                    return false;
                return m_EventPtr->handled;
            }
            set
            {
                if (!valid)
                    throw new NullReferenceException();
                m_EventPtr->handled = value;
            }
        }

        public FourCC type
        {
            get
            {
                if (!valid)
                    return new FourCC();
                return m_EventPtr->type;
            }
        }

        public int sizeInBytes
        {
            get
            {
                if (!valid)
                    return 0;
                return m_EventPtr->sizeInBytes;
            }
        }

        public int deviceId
        {
            get
            {
                if (!valid)
                    return InputDevice.kInvalidDeviceId;
                return m_EventPtr->deviceId;
            }
            set
            {
                if (!valid)
                    throw new NullReferenceException();
                m_EventPtr->deviceId = value;
            }
        }

        public InputDevice device
        {
            get
            {
                if (!valid)
                    return null;
                return InputSystem.TryGetDeviceById(m_EventPtr->deviceId);
            }
            set
            {
                if (!valid)
                    throw new NullReferenceException();
                m_EventPtr->deviceId = device.id;
            }
        }

        public double time
        {
            get
            {
                if (!valid)
                    return 0.0;
                return m_EventPtr->time;
            }
        }

        public IntPtr data
        {
            get
            {
                if (!valid)
                    return IntPtr.Zero;
                return new IntPtr(m_EventPtr);
            }
        }

        public bool IsA<TOtherEvent>()
            where TOtherEvent : struct, IInputEventTypeInfo
        {
            if (m_EventPtr == null)
                return false;

            var otherEventTypeCode = new TOtherEvent().GetTypeStatic();
            return m_EventPtr->type == otherEventTypeCode;
        }
    }
}
