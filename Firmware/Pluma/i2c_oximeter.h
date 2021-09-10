#ifndef _I2C_OXIMETER_H_
#define _I2C_OXIMETER_H_

#include "i2c.h"

//*****************************************************************************
// Prototypes
//*****************************************************************************
uint8_t oximeter_write_byte(i2c_dev_t* dev, uint8_t family, uint8_t index, uint8_t byte, uint8_t delay_ms);
uint8_t oximeter_read_byte(i2c_dev_t* dev, uint8_t family, uint8_t index, uint8_t byte, uint8_t * byte_read);
uint8_t oximeter_read_all(i2c_dev_t* dev, uint8_t * data);
uint8_t oximeter_read_all_step1(i2c_dev_t* dev);
uint8_t oximeter_read_all_step2(i2c_dev_t* dev, uint8_t * data);

#endif /* _I2C_OXIMETER_H_ */
