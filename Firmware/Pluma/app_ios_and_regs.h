#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// ECG_LOM                Description: ECG LO- signal
// ECG_LOP                Description: ECG LO+ signal

#define read_ECG_LOM read_io(PORTC, 0)          // ECG_LOM
#define read_ECG_LOP read_io(PORTC, 1)          // ECG_LOP

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
/* BNC_IOS_DIR */
#define set_BNC_IOS_DIR set_io(PORTD, 7)
#define clr_BNC_IOS_DIR clear_io(PORTD, 7)

/* BNC_IOS_EN */
#define set_BNC_IOS_EN set_io(PORTD, 6)
#define clr_BNC_IOS_EN clear_io(PORTD, 6)

/* OXIMETER_IOS*/
#define set_OXIMETER_RESET set_io(PORTD, 4)
#define clr_OXIMETER_RESET clear_io(PORTD, 4)
#define set_OXIMETER_MFIO set_io(PORTD, 3)
#define clr_OXIMETER_MFIO clear_io(PORTD, 3)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint8_t REG_STREAM_ENABLE;
	uint8_t REG_STREAM_DISABLE;
	uint8_t REG_STREAM_OXIMETER[4];
	uint16_t REG_STREAM_ECG;
	uint16_t REG_STREAM_GSR;
	int16_t REG_STREAM_MOTION[22];
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_STREAM_ENABLE               32 // U8     Writing 1 to the bitmask starts the correspondent data stream
#define ADD_REG_STREAM_DISABLE              33 // U8     Writing 1 to the bitmask stops the correspondent data stream
#define ADD_REG_STREAM_OXIMETER             34 // U8     Contains the oximeter sensor data
#define ADD_REG_STREAM_ECG                  35 // U16    Contains the Ecg analog data
#define ADD_REG_STREAM_GSR                  36 // U16    Contains the GSR analog data
#define ADD_REG_STREAM_MOTION               37 // I16    Contains the motion sensor data

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x25
#define APP_NBYTES_OF_REG_BANK              54

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_STREAM_OXIMETER                  (1<<0)       // Data stream of the oximeter and heartrate
#define B_STREAM_ECG                       (1<<1)       // Data stream of the ECG
#define B_STREAM_GSR                       (1<<2)       // Data stream of the GSR
#define B_STREAM_MOTION                    (1<<3)       // Data stream from the motion sensor

#endif /* _APP_REGS_H_ */