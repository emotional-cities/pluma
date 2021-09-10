#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"


/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_STREAM_ENABLE,
	&app_read_REG_STREAM_DISABLE,
	&app_read_REG_STREAM_OXIMETER,
	&app_read_REG_STREAM_ECG,
	&app_read_REG_STREAM_GSR,
	&app_read_REG_STREAM_MOTION
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_STREAM_ENABLE,
	&app_write_REG_STREAM_DISABLE,
	&app_write_REG_STREAM_OXIMETER,
	&app_write_REG_STREAM_ECG,
	&app_write_REG_STREAM_GSR,
	&app_write_REG_STREAM_MOTION
};


/************************************************************************/
/* REG_STREAM_ENABLE                                                    */
/************************************************************************/
void app_read_REG_STREAM_ENABLE(void)
{
	//app_regs.REG_STREAM_ENABLE = 0;

}

bool app_write_REG_STREAM_ENABLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_STREAM_ENABLE = reg;
	return true;
}


/************************************************************************/
/* REG_STREAM_DISABLE                                                   */
/************************************************************************/
void app_read_REG_STREAM_DISABLE(void)
{
	//app_regs.REG_STREAM_DISABLE = 0;

}

bool app_write_REG_STREAM_DISABLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_STREAM_DISABLE = reg;
	return true;
}


/************************************************************************/
/* REG_STREAM_OXIMETER                                                  */
/************************************************************************/
// This register is an array with 4 positions
void app_read_REG_STREAM_OXIMETER(void)
{
	//app_regs.REG_STREAM_OXIMETER[0] = 0;

}

bool app_write_REG_STREAM_OXIMETER(void *a)
{
	uint8_t *reg = ((uint8_t*)a);

	app_regs.REG_STREAM_OXIMETER[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_STREAM_ECG                                                       */
/************************************************************************/
void app_read_REG_STREAM_ECG(void)
{
	//app_regs.REG_STREAM_ECG = 0;

}

bool app_write_REG_STREAM_ECG(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_STREAM_ECG = reg;
	return true;
}


/************************************************************************/
/* REG_STREAM_GSR                                                       */
/************************************************************************/
void app_read_REG_STREAM_GSR(void)
{
	//app_regs.REG_STREAM_GSR = 0;

}

bool app_write_REG_STREAM_GSR(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_STREAM_GSR = reg;
	return true;
}


/************************************************************************/
/* REG_STREAM_MOTION                                                    */
/************************************************************************/
// This register is an array with 22 positions
void app_read_REG_STREAM_MOTION(void)
{
	//app_regs.REG_STREAM_MOTION[0] = 0;

}

bool app_write_REG_STREAM_MOTION(void *a)
{
	int16_t *reg = ((int16_t*)a);

	app_regs.REG_STREAM_MOTION[0] = reg[0];
	return true;
}