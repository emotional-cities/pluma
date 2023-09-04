#include "i2c_MotionSens_bno055.h"
#include "i2c.h"
#include "string.h"

i2c_dev_t bno055;


#include "app_ios_and_regs.h"

uint8_t addd;
uint8_t myArray[16];

uint8_t asd =0;

bool initialize_bno055(void)
{
	i2c0_init();
	
	/* Initialize State LED in case we want to use it */
	io_pin2out(&PORTD, 5, PULL_IO_DOWN, SENSE_IO_LOW_LEVEL);
	
	/* Set BNO055 address */
	bno055.add = 0x28;
	
	/* Check if we can read the ID */
	bno055.reg = BNO055_CHIP_ID_ADDR;
	
	//while(1)
	//{
		
		//bno055_set_mode(OPERATION_MODE_AMG);
		i2c0_rReg(&bno055, 4);
		//_delay_ms(20);
	//}
	
	if (bno055.reg_val != BNO055_ID)
	{
		/* Try again */
		_delay_ms(1000);
		i2c0_rReg(&bno055, 2);
		
		if (bno055.reg_val != BNO055_ID)
		{
			/* Try again */
			_delay_ms(1000);
			i2c0_rReg(&bno055, 2);
			
			if (bno055.reg_val != BNO055_ID)
			{
				/* Not able to read the ID */
				return false;
			}
		}
	}
	
	/* Switch to config mode (just in case since this is the default) */
	/*
	if (bno055_set_mode(OPERATION_MODE_CONFIG) == false)
		return false;
	_delay_ms(20);
	*/
	
	/* Reset device */
	bno055.reg = BNO055_SYS_TRIGGER_ADDR;
	bno055.reg_val = 0x20;
	if (i2c0_wReg(&bno055) == false)
	{
		_delay_ms(200);
		
		if (i2c0_wReg(&bno055) == false)
		{
			_delay_ms(200);
			return false;
		}
	}
	_delay_ms(200);
	
	/* Wait until we can read the ID again */
	bno055.reg = BNO055_CHIP_ID_ADDR;
	i2c0_rReg(&bno055, 2);
	while(bno055.reg_val != BNO055_ID)
	{
		i2c0_rReg(&bno055, 2);
		_delay_ms(10);
	}
	_delay_ms(50);
	
	/* Configure to normal power mode */
	/*
	bno055.reg = BNO055_PWR_MODE_ADDR;
	bno055.reg_val = POWER_MODE_NORMAL;
	i2c0_wReg(&bno055);
	_delay_ms(20);
	if (i2c0_wReg(&bno055) == false)
		return false;
	_delay_ms(20);
	*/
	
	/*
	bno055.reg = BNO055_PAGE_ID_ADDR;
	bno055.reg_val = 0;
	if (i2c0_wReg(&bno055) == false)
		return false;
	_delay_ms(20);
	*/
	
	/*
	bno055.reg = BNO055_SYS_TRIGGER_ADDR;
	bno055.reg_val = 0;
	if (i2c0_wReg(&bno055) == false)
		return false;
	_delay_ms(20);
	*/
	
	/* Set the operating mode */
	//if (bno055_set_mode(OPERATION_MODE_NDOF) == false)
	//if (bno055_set_mode(OPERATION_MODE_AMG) == false)
	if (bno055_set_mode(OPERATION_MODE_ACCONLY) == false)
		return false;
	_delay_ms(20);
	
	while(asd == 0)
	{
		//bno055_read_vector(BNO055_EULER_H_LSB_ADDR, &myArray);
		//bno055_set_mode(OPERATION_MODE_ACCONLY);
		//bno055_set_mode(OPERATION_MODE_ACCONLY);
		
		//bno055_read_vector(BNO055_ACCEL_DATA_X_LSB_ADDR, &myArray);
		
		
		bno055.reg = BNO055_OPR_MODE_ADDR;
		bno055.reg_val = OPERATION_MODE_ACCONLY;
		
		i2c0_rReg(&bno055, 2);
		i2c0_rReg(&bno055, 2);
		
		i2c0_wReg(&bno055);
		_delay_ms(20);
		i2c0_wReg(&bno055);
		_delay_ms(20);
		i2c0_wReg(&bno055);
		_delay_ms(20);
		i2c0_wReg(&bno055);
		_delay_ms(20);
		
		i2c0_rReg(&bno055, 2);
		i2c0_rReg(&bno055, 2);
		
		
		bno055.reg = BNO055_OPR_MODE_ADDR;
		i2c0_rReg(&bno055, 1);
		i2c0_rReg(&bno055, 1);
		
		//bno055_read_vector(BNO055_ACCEL_DATA_X_LSB_ADDR, &myArray);
		//bno055_read_vector(BNO055_ACCEL_DATA_X_LSB_ADDR, &myArray);
	}
	
	while(1)
	{
		bno055.reg = BNO055_OPR_MODE_ADDR;
		i2c0_rReg(&bno055, 2);
		i2c0_rReg(&bno055, 2);
		
		bno055_read_vector(BNO055_ACCEL_DATA_X_LSB_ADDR, &myArray);
		bno055_read_vector(BNO055_ACCEL_DATA_X_LSB_ADDR, &myArray);
	}
	
	return true;
}

bool bno055_set_mode(uint8_t mode)
{
	bno055.reg = BNO055_OPR_MODE_ADDR;
	bno055.reg_val = mode;
	i2c0_wReg(&bno055);
	_delay_ms(20);
	if (i2c0_wReg(&bno055) == false)
		return false;
	
	i2c0_rReg(&bno055, 2);
	if (bno055.reg_val != mode)
		return false;
		
	return true;	
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
