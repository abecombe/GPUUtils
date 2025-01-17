#ifndef GPU_UTILS_DISPATCH_HELPER_HLSL
#define GPU_UTILS_DISPATCH_HELPER_HLSL

//#pragma multi_compile _ DIRECT_DISPATCH INDIRECT_DISPATCH

#if !defined(DIRECT_DISPATCH) && !defined(INDIRECT_DISPATCH)
#define DIRECT_DISPATCH
#endif

// for direct dispatch
#if defined(DIRECT_DISPATCH)

uint3 _DispatchThreadSize;
#define RETURN_IF_INVALID_THREAD(TID)\
const uint3 dispatch_thread_size = _DispatchThreadSize;\
if (any(TID >= dispatch_thread_size)) return;\

#define GET_DISPATCH_THREAD_SIZE()\
_DispatchThreadSize\

// for indirect dispatch
#elif defined(INDIRECT_DISPATCH)

StructuredBuffer<uint3> _DispatchThreadSizeBuffer;
#define RETURN_IF_INVALID_THREAD(TID)\
const uint3 dispatch_thread_size = _DispatchThreadSizeBuffer[0];\
if (any(TID >= dispatch_thread_size)) return;\

#define GET_DISPATCH_THREAD_SIZE()\
_DispatchThreadSizeBuffer[0]\

#endif


#endif /* GPU_UTILS_DISPATCH_HELPER_HLSL */