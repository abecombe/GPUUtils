#ifndef GPU_UTILS_BUFFER_UTILS_HLSL
#define GPU_UTILS_BUFFER_UTILS_HLSL

// GPUStructuredBuffer
#define STRUCTURED_BUFFER_DATA_CONSTANT(BUFFER)\
int BUFFER##Length;\
int3 BUFFER##Size;\
int3 BUFFER##StartIndex;\
int3 BUFFER##EndIndex;\
float3 BUFFER##IndexToPosition;\
float3 BUFFER##PositionToIndex;\
float3 BUFFER##PositionToFloorIndex;\

#define BUFFER_SIZE(BUFFER)\
BUFFER##Size\

#define BUFFER_LENGTH(BUFFER)\
BUFFER##Length\

#define BUFFER_START_INDEX(BUFFER)\
BUFFER##StartIndex\

#define BUFFER_END_INDEX(BUFFER)\
BUFFER##EndIndex\

#define BUFFER_POSITION_OFFSET(BUFFER)\
BUFFER##PositionToIndex\

#define FUNCTION_INDEX_TO_ID(BUFFER)\
inline uint IndexToID##BUFFER(in int3 index)\
{ index = clamp(index, BUFFER##StartIndex, BUFFER##EndIndex); int3 offset = index - BUFFER##StartIndex; return offset.x + (offset.y + offset.z * BUFFER##Size.y) * BUFFER##Size.x; }\

#define INDEX_TO_ID(BUFFER, INDEX)\
IndexToID##BUFFER(INDEX)\

#define IS_VALID_ID(BUFFER, ID)\
(ID >= 0 && ID < BUFFER##Length)\

#define IS_VALID_INDEX(BUFFER, INDEX)\
(all(INDEX >= BUFFER##StartIndex) && all(INDEX <= BUFFER##EndIndex))\

#define INDEX_TO_POSITION(BUFFER, INDEX)\
(BUFFER##IndexToPosition + (float3)INDEX)\

#define POSITION_TO_INDEX(BUFFER, POSITION)\
(floor(POSITION + BUFFER##PositionToIndex))\

#define POSITION_TO_ID(BUFFER, POSITION)\
IndexToID##BUFFER(POSITION_TO_INDEX(BUFFER, POSITION))\

#define POSITION_TO_FLOOR_INDEX(BUFFER, POSITION)\
(floor(POSITION + BUFFER##PositionToFloorIndex))\

#define POSITION_TO_FLOOR_INDEX_RATIO(BUFFER, POSITION)\
(1 - frac(POSITION + BUFFER##PositionToFloorIndex))\

#define FUNCTION_READ_DATA(BUFFER, TYPE)\
inline TYPE ReadData##BUFFER(uint id)\
{ return BUFFER[id]; }\
inline TYPE ReadData##BUFFER(int id)\
{ return BUFFER[id]; }\
inline TYPE ReadData##BUFFER(uint3 index)\
{ return BUFFER[INDEX_TO_ID(BUFFER, index)]; }\
inline TYPE ReadData##BUFFER(int3 index)\
{ return BUFFER[INDEX_TO_ID(BUFFER, index)]; }\

#define FUNCTION_WRITE_DATA(BUFFER, TYPE)\
inline void WriteData##BUFFER(uint id, TYPE value)\
{ BUFFER[id] = value; }\
inline void WriteData##BUFFER(int id, TYPE value)\
{ BUFFER[id] = value; }\
inline void WriteData##BUFFER(uint3 index, TYPE value)\
{ BUFFER[INDEX_TO_ID(BUFFER, index)] = value; }\
inline void WriteData##BUFFER(int3 index, TYPE value)\
{ BUFFER[INDEX_TO_ID(BUFFER, index)] = value; }\

#define READ_BUFFER(TYPE, BUFFER)\
StructuredBuffer<TYPE> BUFFER;\
STRUCTURED_BUFFER_DATA_CONSTANT(BUFFER)\
FUNCTION_INDEX_TO_ID(BUFFER)\
FUNCTION_READ_DATA(BUFFER, TYPE)\

#define WRITE_BUFFER(TYPE, BUFFER)\
RWStructuredBuffer<TYPE> BUFFER;\
STRUCTURED_BUFFER_DATA_CONSTANT(BUFFER)\
FUNCTION_INDEX_TO_ID(BUFFER)\
FUNCTION_WRITE_DATA(BUFFER, TYPE)\

#define RW_BUFFER(TYPE, BUFFER)\
RWStructuredBuffer<TYPE> BUFFER;\
STRUCTURED_BUFFER_DATA_CONSTANT(BUFFER)\
FUNCTION_INDEX_TO_ID(BUFFER)\
FUNCTION_READ_DATA(BUFFER, TYPE)\
FUNCTION_WRITE_DATA(BUFFER, TYPE)\

#define READ_DATA(BUFFER, ID_OR_INDEX)\
ReadData##BUFFER(ID_OR_INDEX)\

#define WRITE_DATA(BUFFER, ID_OR_INDEX, VALUE)\
WriteData##BUFFER(ID_OR_INDEX, VALUE);\

#define BUFFER_POINTER_WITH_ID(BUFFER, ID)\
BUFFER[ID]\

#define BUFFER_POINTER_WITH_INDEX(BUFFER, INDEX)\
BUFFER[INDEX_TO_ID(BUFFER, INDEX)]\

// GPUIndirectArgumentsBuffer
#define ARGS_BUFFER_DATA_CONSTANT(BUFFER)\
int BUFFER##CountBufferOffset;\
int BUFFER##CountBufferSize;\

#define FUNCTION_GET_ARGS_BUFFER_COUNT(BUFFER)\
inline uint3 GetArgsBufferCount##BUFFER()\
{\
switch (BUFFER##CountBufferSize) {\
case 1: return uint3(BUFFER[BUFFER##CountBufferOffset], 0, 0);\
case 2: return uint3(BUFFER[BUFFER##CountBufferOffset], BUFFER[BUFFER##CountBufferOffset + 1], 0);\
case 3: return uint3(BUFFER[BUFFER##CountBufferOffset], BUFFER[BUFFER##CountBufferOffset + 1], BUFFER[BUFFER##CountBufferOffset + 2]);\
default: return uint3(0, 0, 0); }\
}\

#define FUNCTION_SET_ARGS_BUFFER_COUNT(BUFFER)\
inline void SetArgsBufferCount##BUFFER(uint count)\
{ BUFFER[BUFFER##CountBufferOffset] = count; }\
inline void SetArgsBufferCount##BUFFER(uint2 count)\
{ BUFFER[BUFFER##CountBufferOffset] = count.x; BUFFER[BUFFER##CountBufferOffset + 1] = count.y; }\
inline void SetArgsBufferCount##BUFFER(uint3 count)\
{ BUFFER[BUFFER##CountBufferOffset] = count.x; BUFFER[BUFFER##CountBufferOffset + 1] = count.y; BUFFER[BUFFER##CountBufferOffset + 2] = count.z; }\

#define ARGS_READ_BUFFER(BUFFER)\
StructuredBuffer<uint> BUFFER;\
ARGS_BUFFER_DATA_CONSTANT(BUFFER)\
FUNCTION_GET_ARGS_BUFFER_COUNT(BUFFER)\

#define ARGS_WRITE_BUFFER(BUFFER)\
RWStructuredBuffer<uint> BUFFER;\
ARGS_BUFFER_DATA_CONSTANT(BUFFER)\
FUNCTION_SET_ARGS_BUFFER_COUNT(BUFFER)\

#define ARGS_RW_BUFFER(BUFFER)\
RWStructuredBuffer<uint> BUFFER;\
ARGS_BUFFER_DATA_CONSTANT(BUFFER)\
FUNCTION_GET_ARGS_BUFFER_COUNT(BUFFER)\
FUNCTION_SET_ARGS_BUFFER_COUNT(BUFFER)\

#define READ_ARGS_DATA(BUFFER, ID)\
BUFFER[ID]\

#define WRITE_ARGS_DATA(BUFFER, ID, VALUE)\
BUFFER[ID] = VALUE;\

// return uint3 count, so use .x, .xy, .xyz
#define GET_ARGS_BUFFER_COUNT(BUFFER)\
GetArgsBufferCount##BUFFER()\

#define SET_ARGS_BUFFER_COUNT(BUFFER, VALUE)\
SetArgsBufferCount##BUFFER(VALUE);\

// GPUAppendConsumeBuffer
#define APPEND_BUFFER(TYPE, BUFFER)\
AppendStructuredBuffer<TYPE> BUFFER;\

#define CONSUME_BUFFER(TYPE, BUFFER)\
ConsumeStructuredBuffer<TYPE> BUFFER;\
StructuredBuffer<uint> BUFFER##CountBuffer;\

#define APPEND_DATA(BUFFER, VALUE)\
BUFFER.Append(VALUE);\

#define CONSUME_DATA(BUFFER)\
BUFFER.Consume()\

#define BUFFER_STACK_COUNT(BUFFER)\
BUFFER##CountBuffer[0]\

// GPUCounterBuffer
#define COUNTER_BUFFER(BUFFER)\
RWStructuredBuffer<uint> BUFFER;\

#define COUNTER_BUFFER_COUNT(BUFFER)\
BUFFER[0]\

#define INCREMENT_COUNTER(BUFFER)\
BUFFER.IncrementCounter()\

#define DECREMENT_COUNTER(BUFFER)\
BUFFER.DecrementCounter()\

// GPUConstantBuffer
#define CONSTANT_BUFFER(TYPE, BUFFER)\
ConstantBuffer<TYPE> BUFFER;\


#endif /* GPU_UTILS_BUFFER_UTILS_HLSL */