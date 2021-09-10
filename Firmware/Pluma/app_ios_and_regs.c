#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTC, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // ECG_LOM
	io_pin2in(&PORTC, 1, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // ECG_LOP

	/* Configure input interrupts */
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<0), false);                 // ECG_LOM
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<1), false);                 // ECG_LOP



	/* Configure output pins */
	io_pin2out(&PORTD, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // BNC_IOS_DIR
	io_pin2out(&PORTD, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // BNC_IOS_EN
	io_pin2out(&PORTD, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // oximeter_hrensor \MFIC
	io_pin2out(&PORTD, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // oximeter_hrsensor \RESET


	/* Initialize output pins */
	clr_BNC_IOS_DIR;
	clr_BNC_IOS_EN;
	set_OXIMETER_MFIO;
	set_OXIMETER_RESET;		// OK = SET
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
	TYPE_I16
};

uint16_t app_regs_n_elements[] = {
	1,
	1,
	4,
	1,
	1,
	22
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_STREAM_ENABLE),
	(uint8_t*)(&app_regs.REG_STREAM_DISABLE),
	(uint8_t*)(app_regs.REG_STREAM_OXIMETER),
	(uint8_t*)(&app_regs.REG_STREAM_ECG),
	(uint8_t*)(&app_regs.REG_STREAM_GSR),
	(uint8_t*)(app_regs.REG_STREAM_MOTION)
};