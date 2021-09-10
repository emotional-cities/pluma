#include "i2c_bno055.h"
#include "i2c.h"
#include "string.h"

i2c_dev_t bno055;

bool initialize_bno055(void)
{
	i2c0_init();
	
	// Set BNO055 address */
	bno055.add = 0x28;
	
	/* Check if we can read the ID */
	bno055.reg = BNO055_CHIP_ID_ADDR;
	i2c0_rReg(&bno055, 1);
	if (bno055.reg_val != 55)
	{
		_delay_ms(1000);
		
		i2c0_rReg(&bno055, 1);
		if (bno055.reg_val != 55)
		{
			return false;
		}
	}
	
	/* Switch to config mode (just in case since this is the default) */
	bno055_set_mode(OPERATION_MODE_CONFIG);
	
	/* Reset */
	bno055.reg = BNO055_SYS_TRIGGER_ADDR;
	bno055.reg_val = 0x20;
	if (i2c0_wReg(&bno055) == false)
		return false;
	_delay_ms(1000);
	
	/* Set to normal power mode */
	bno055.reg = BNO055_PWR_MODE_ADDR;
	bno055.reg_val = POWER_MODE_NORMAL;
	if (i2c0_wReg(&bno055) == false)
		return false;
	_delay_ms(10);

	bno055.reg = BNO055_PAGE_ID_ADDR;
	bno055.reg_val = 0;
	if (i2c0_wReg(&bno055) == false)
		return false;
	
	bno055.reg = BNO055_SYS_TRIGGER_ADDR;
	bno055.reg_val = 0x0;
	if (i2c0_wReg(&bno055) == false)
		return false;
	_delay_ms(10);
	
	/* Set the operating mode */
	bno055_set_mode(OPERATION_MODE_NDOF);
	_delay_ms(20);
	
	return true;
}

void bno055_set_mode(uint8_t mode)
{
	bno055.reg_val = mode;
	i2c0_wReg(&bno055);
	_delay_ms(30);
}

bool bno055_read_vector(uint8_t vector, void * array)
{
	bno055.reg = vector;
	
	switch (vector)
	{
		case BNO055_ACCEL_DATA_X_LSB_ADDR:
		case BNO055_MAG_DATA_X_LSB_ADDR:
		case BNO055_GYRO_DATA_X_LSB_ADDR:
		case BNO055_EULER_H_LSB_ADDR:
		case BNO055_LINEAR_ACCEL_DATA_X_LSB_ADDR:
		case BNO055_GRAVITY_DATA_X_LSB_ADDR:			 
			 
			 if (i2c0_rReg(&bno055, 6) == false)
				return false;
			 
			 memcpy(array, bno055.data, 6);
			 return true;			 
			 
		case BNO055_QUATERNION_DATA_W_LSB_ADDR:
			 
			 if (i2c0_rReg(&bno055, 8) == false)
				return false;
				
			 memcpy(array, bno055.data, 8);
			 return true;
	}
	
	return false;
}