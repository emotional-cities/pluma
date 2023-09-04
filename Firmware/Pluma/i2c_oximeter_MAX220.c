#include "i2c_oximeter_MAX220.h"



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



uint8_t oximeter_write_byte(i2c_dev_t* dev, uint8_t family, uint8_t index, uint8_t byte, uint8_t delay_ms)
{
	dev->data[0] = family;
	dev->data[1] = index;
	dev->data[2] = byte;
	
	if (i2c0_wArray(dev, 3) == false)
	return 1;
	
	for (uint8_t i = 0; i < delay_ms; i++)
	_delay_ms(1);
	
	if (i2c0_rArray(dev, 1) == false)
	return 1;
	
	return dev->reg_val;
}

uint8_t oximeter_read_byte(i2c_dev_t* dev, uint8_t family, uint8_t index, uint8_t byte, uint8_t * byte_read)
{
	dev->data[0] = family;
	dev->data[1] = index;
	dev->data[2] = byte;
	
	if (i2c0_wArray(dev, 3) == false)
	return 1;
	
	_delay_ms(6);
	
	if (i2c0_rArray(dev, 2) == false)
	return 1;
	
	*byte_read = dev->data[1];
	
	return dev->data[0];
}

uint8_t oximeter_read_all(i2c_dev_t* dev, uint8_t * data)
{
	dev->data[0] = READ_DATA_OUTPUT;
	dev->data[1] = READ_DATA;
	
	uint8_t read_length = 6;
	
	if (i2c0_wArray(dev, 2) == false)
	return 1;
	
	_delay_ms(10);
	
	if (i2c0_rArray(dev, read_length + 1) == false)
	return 1;
	
	for (uint8_t i = 0; i < read_length; i++)
	*(data+i) = dev->data[i+1];
	
	return dev->data[0];
}

uint8_t oximeter_read_all_step1(i2c_dev_t* dev)
{
	dev->data[0] = READ_DATA_OUTPUT;
	dev->data[1] = READ_DATA;
	
	if (i2c0_wArray(dev, 2) == false)
	return 1;
	
	return 0;
}

uint8_t oximeter_read_all_step2(i2c_dev_t* dev, uint8_t * data)
{
	dev->data[0] = READ_DATA_OUTPUT;
	dev->data[1] = READ_DATA;
	
	uint8_t read_length = 6;
	
	if (i2c0_rArray(dev, read_length + 1) == false)
	return 1;
	
	for (uint8_t i = 0; i < read_length; i++)
	*(data+i) = dev->data[i+1];
	
	return dev->data[0];
}


