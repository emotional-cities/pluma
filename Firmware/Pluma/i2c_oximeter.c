#include "i2c_oximeter.h"
#include "oximeter_hrsensor.h"


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


