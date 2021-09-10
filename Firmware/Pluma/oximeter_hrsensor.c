#include "oximeter_hrsensor.h"
#include "i2c_oximeter.h"
#include "app_ios_and_regs.h"
#include "i2c_user.h"
#include "i2c.h"


i2c_dev_t oximeter;

void initialize_oximeter_hrsensor()
{	
	i2c0_init();
	
	for(uint8_t i=0; i<10; i++){	//min: 10*100ms
		_delay_ms(100);
	}

	//structure initialize
	oximeter.add = OXIMETER_ADDRESS;
	oximeter.reg = 0;
	oximeter.reg_val=10;
	
	
	// Family Byte: OUTPUT_MODE (0x10), 
	// Index Byte: SET_FORMAT (0x00),
	// Write Byte : outputType (Parameter values in OUTPUT_MODE_WRITE_BYTE)
	if (oximeter_write_byte(&oximeter, OUTPUT_MODE, SET_FORMAT, ALGO_DATA, 45) == 0)				// not critical
	{ /* SUCCESS*/ }
	
	// Family Byte: OUTPUT_MODE(0x10), 
	// Index Byte: WRITE_SET_THRESHOLD (0x01), 
	// Write byte: intThres (parameter - value between 0 and 0xFF).
	// This function changes the threshold for the FIFO interrupt bit/pin. The
	// interrupt pin is the MFIO pin which is set to INPUT after IC initialization
	if (oximeter_write_byte(&oximeter, OUTPUT_MODE, WRITE_SET_THRESHOLD, 1, 45) == 0) {}			// not critical
	{ /* SUCCESS*/ }

	// Family Byte: ENABLE_ALGORITHM (0x52), 
	// Index Byte:ENABLE_AGC_ALGO (0x00)
	// This function enables (one) or disables (zero) the automatic gain control algorithm.
	if (oximeter_write_byte(&oximeter, ENABLE_ALGORITHM, ENABLE_AGC_ALGO, 1, 45) == 0) {}		// not critical
	{ /* SUCCESS*/ }
	
	// Family Byte: ENABLE_SENSOR (0x44)
	// Index Byte: ENABLE_MAX30101 (0x03)
	// Byte: 0x00 or 0x01 (enable)
	// This function enables the MAX30101.
	if (oximeter_write_byte(&oximeter, ENABLE_SENSOR, ENABLE_MAX30101, 1, 45) == 0)				// !! must be !!!!!!!
	{ /* SUCCESS*/ }
	
	// Family Byte: ENABLE_ALGORITHM (0x52)
	// Index Byte: ENABLE_WHRM_ALGO (0x02)
	// This function enables (one) or disables (zero) the wrist heart rate monitor algorithm.
	if (oximeter_write_byte(&oximeter, ENABLE_ALGORITHM, ENABLE_WHRM_ALGO, 1, 45) == 0)			// !! must be !!!!!!!
	{ /* SUCCESS*/ }
			
	_delay_ms(1000);
}
