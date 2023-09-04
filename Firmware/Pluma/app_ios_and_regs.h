#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
#define read_ECG_LOM read_io(PORTB, 0)          // ECG_LOM: Leads-off-
#define read_ECG_LOP read_io(PORTB, 1)          // ECG_LOP: Leads-off+

#define read_DI0 read_io(PORTC, 4)              // Digital input 0
#define read_DI1 read_io(PORTC, 5)              // Digital input 1

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
/* CLCK_IN/OUT_EN*/
#define set_CLOCK_IN_EN set_io(PORTC, 2)
#define clr_CLOCK_IN_EN clear_io(PORTC, 2)
#define set_CLOCK_OUT_EN set_io(PORTC, 3)
#define clr_CLOCK_OUT_EN clear_io(PORTC, 3)

/* LED_AUX */
#define set_LED_AUX set_io(PORTD, 4)
#define clr_LED_AUX clear_io(PORTD, 4)

/* GPS_RESET */
#define set_GPS_RESET set_io(PORTA, 6)
#define clr_GPS_RESET clear_io(PORTA, 6)
#define clr_BNC_IOS_EN clear_io(PORTD, 6)

/* BNO055_TRIG */
#define set_BNO055_TRIG set_io(PORTD, 2)
#define clr_BNO055_TRIG clear_io(PORTD, 2)

/* Digital output 0 */
#define set_DO0 set_io(PORTB, 2)
#define clr_DO0 clear_io(PORTB, 2)

/* Digital output 1 */
#define set_DO1 set_io(PORTB, 3)
#define clr_DO1 clear_io(PORTB, 3)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint8_t REG_STREAM_ENABLE;
	uint8_t REG_STREAM_DISABLE;
	uint8_t REG_STREAM_OXIMETER[4];
	uint16_t REG_STREAM_ECG[2];
	uint16_t REG_STREAM_GSR;
	uint8_t REG_STREAM_MOTION;
	uint8_t REG_INPUTS;
	uint8_t REG_SET_OUTPUTS;
	uint8_t REG_CLEAR_OUTPUTS;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_STREAM_ENABLE               32 // U8     Writing 1 to the bitmask starts the correspondent data stream
#define ADD_REG_STREAM_DISABLE              33 // U8     Writing 1 to the bitmask stops the correspondent data stream
#define ADD_REG_STREAM_OXIMETER             34 // U8     Contains the oximeter sensor data
#define ADD_REG_STREAM_ECG                  35 // U16    Contains the Ecg analog data	& external Analog_In[0]
#define ADD_REG_STREAM_GSR                  36 // U16    Contains the GSR analog data
#define ADD_REG_STREAM_MOTION               37 // U8     Contains the motion sensor index
#define ADD_REG_INPUTS                      38 // U8     Contains the state of the digital input
#define ADD_REG_SET_OUTPUTS                 39 // U8     Set the digital outputs
#define ADD_REG_CLEAR_OUTPUTS               40 // U8     Clear the digital outputs

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x28
#define APP_NBYTES_OF_REG_BANK              16

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_STREAM_OXIMETER                  (1<<0)       // Data stream of the oximeter and heartrate
#define B_STREAM_ECG                       (1<<1)       // Data stream of the ECG
#define B_STREAM_GSR                       (1<<2)       // Data stream of the GSR
#define B_STREAM_MOTION                    (1<<3)       // Data stream from the motion sensor
#define B_DI0                              (1<<0)       // Digital input 0
#define B_DI1                              (1<<1)       // Digital input 1
#define B_DO0                              (1<<0)       // Digital output 0
#define B_DO1                              (1<<1)       // Digital output 1

#endif /* _APP_REGS_H_ */
