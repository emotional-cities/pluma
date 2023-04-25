#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	
	/* Configure input pins */
	//io_pin2out(&PORTA, 0, PULL_IO_UP, SENSE_IO_LOW_LEVEL);			// gets to be ADC 3V3 ref		open GPIO
	//io_pin2out(&PORTA, 1, PULL_IO_UP, SENSE_IO_LOW_LEVEL);				// open GPIO
	io_pin2out(&PORTA, 2, PULL_IO_DOWN, SENSE_IO_LOW_LEVEL);			// open GPIO
	io_pin2out(&PORTA, 3, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		// Aux_In_1
	io_pin2out(&PORTA, 4, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		// Aux_In_2
	//io_pin2out(&PORTA, 5, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		// PA5 = Analog in from << HR_out <<<<<<<<<<<<<<< to Set Up
	//io_pin2out(&PORTA, 7, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		// PA7 = Analog in from << EDA_out <<<<<<<<<<<<<<< to Set Up

	io_pin2out(&PORTB, 0, PULL_IO_UP, SENSE_IO_EDGES_BOTH);				//HR_LO- Leads_off- detect io
	io_pin2out(&PORTB, 1, PULL_IO_UP, SENSE_IO_EDGES_BOTH);				//HR_LO+ Leads_off+ detect io
	

	//io_pin2in(&PORTC, 0, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		//ECG_LOM????	I2C_SDA port#1
	//io_pin2in(&PORTC, 1, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		//ECG_LOP???	I2C SCL port#1
	//io_pin2in(&PORTC, 4, PULL_IO_UP, SENSE_IO_LOW_LEVEL);				//open GPIO
	//io_pin2in(&PORTC, 5, PULL_IO_UP, SENSE_IO_LOW_LEVEL);				//open GPIO
	//io_pin2in(&PORTC, 6, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		//Tx output to GPS
	//io_pin2in(&PORTC, 7, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		//Rx input to GPS
	
	//io_pin2in(&PORTD, 0, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		//I2C_SDA port#2
	//io_pin2in(&PORTD, 1, PULL_IO_TRISTATE, SENSE_IO_LOW_LEVEL);		//I2C SCL port#2
	//io_pin2out(&PORTD, 2, PULL_IO_DOWN, SENSE_IO_LOW_LEVEL);			//open GPIO
	io_pin2out(&PORTD, 3, PULL_IO_DOWN, SENSE_IO_LOW_LEVEL);			//open GPIO
	io_pin2out(&PORTD, 4, PULL_IO_DOWN, SENSE_IO_LOW_LEVEL);            // open GPIO (on schm LED_AUX but n.c.)
	io_pin2out(&PORTD, 6, PULL_IO_DOWN, SENSE_IO_EDGES_BOTH);			// CLOCK_IN
	
	io_pin2out(&PORTD, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // BNO055_TRIG
	io_pin2out(&PORTB, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // Digital output 0
	io_pin2out(&PORTB, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // Digital output 1
	
	io_pin2in(&PORTC, 4, PULL_IO_DOWN, SENSE_IO_EDGES_BOTH);             // Digital input 0
	io_pin2in(&PORTC, 5, PULL_IO_DOWN, SENSE_IO_EDGES_BOTH);             // Digital input 0

	
	/* Configure input interrupts */
	//io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);            // HR_LO-	??
	//io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<1), false);            // HR_LO+	??
	
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<4), false);                 // Digital input 0
	io_set_int(&PORTC, INT_LEVEL_LOW, 1, (1<<5), false);                 // Digital input 1
	






	/* Configure output pins */	
	io_pin2out(&PORTA, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                 // GPS_Reset
	io_pin2out(&PORTA, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);					//EDA_RESET
	
	io_pin2out(&PORTC, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);					//EN_CLOCK_IN
	io_pin2out(&PORTC, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);					//EN_CLOCK_OUT	

	io_pin2out(&PORTD, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CLOCK_OUT

	/* Initialize output pins */
	set_LED_AUX;
	clr_GPS_RESET;
	set_CLOCK_IN_EN;	//buffer off
	set_CLOCK_OUT_EN;	//buffer off
	
	clr_BNO055_TRIG;
}



/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8
};

uint16_t app_regs_n_elements[] = {
	1,
	1,
	4,
	2,
	1,
	1,
	1,
	1,
	1
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_STREAM_ENABLE),
	(uint8_t*)(&app_regs.REG_STREAM_DISABLE),
	(uint8_t*)(app_regs.REG_STREAM_OXIMETER),
	(uint8_t*)(app_regs.REG_STREAM_ECG),
	(uint8_t*)(&app_regs.REG_STREAM_GSR),
	(uint8_t*)(&app_regs.REG_STREAM_MOTION),
	(uint8_t*)(&app_regs.REG_INPUTS),
	(uint8_t*)(&app_regs.REG_SET_OUTPUTS),
	(uint8_t*)(&app_regs.REG_CLEAR_OUTPUTS)
};