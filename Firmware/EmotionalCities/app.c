#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"


#include "i2c_oximeter_MAX220.h"
#include "i2c_MotionSens_bno055.h"


/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);
extern i2c_dev_t oximeter;

/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "Pluma";
uint16_t heartRate;
uint8_t confidence;
uint16_t oxygen;
uint8_t status;
uint8_t data[6];



void hwbp_app_initialize(void)
{
    /* Define versions */
    uint8_t hwH = 0;
    uint8_t hwL = 1;
    uint8_t fwH = 0;
    uint8_t fwL = 5;
    uint8_t ass = 0;
    
   	/* Start core */
    core_func_start_core(
        2110,
        hwH, hwL,
        fwH, fwL,
        ass,
        (uint8_t*)(&app_regs),
        APP_NBYTES_OF_REG_BANK,
        APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
        default_device_name
    );
}

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	
}

/************************************************************************/
/* User functions                                                       */
/************************************************************************/
/* Add your functions here or load external functions if needed */

/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
bool i_;
void core_callback_1st_config_hw_after_boot(void)
{
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
	
	/* Initialize hardware */
	adc_A_initialize_single_ended(REF_ADC_PORTA);
	
	/* Initialize Oximeter */
	initialize_oximeter_hrsensor();
	
	/* Initialize BNO055 */
	//i_ = initialize_bno055();
	//init_ios();
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	
}

void core_callback_registers_were_reinitialized(void)
{
	/* Update registers if needed */
	
	/*TWIC.CTRL = TWI_SDAHOLD_OFF_gc;							// Remove SDA hold time
	TWIC.MASTER.CTRLA  = TWI_MASTER_INTLVL_LO_gc;			// Set interrupts to low level
	TWIC.MASTER.CTRLA |= TWI_MASTER_RIEN_bm;				// Enable read interrupt enable
	TWIC.MASTER.CTRLA |= TWI_MASTER_WIEN_bm;				// Enable write interrupt enable
	TWIC.MASTER.CTRLA |= TWI_MASTER_ENABLE_bm;				// Set TWI as Master
	TWIC.MASTER.CTRLB  = TWI_MASTER_TIMEOUT_DISABLED_gc;	// Disable inactive bus timeout
	*/

}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void) {}
void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
uint16_t ms_counter = 0;
uint8_t motion_counter = 0;

void core_callback_t_before_exec(void) {}
void core_callback_t_after_exec(void) {}

void core_callback_t_new_second(void)
{
	ms_counter = 0;
}

void core_callback_t_500us(void) {}

void core_callback_t_1ms(void)
{
	clr_BNO055_TRIG;
	
	core_func_mark_user_timestamp();
	
	/* MOTION external trigger */
	if (app_regs.REG_STREAM_ENABLE & B_STREAM_MOTION)
	{
		if ((ms_counter % (1000/50)) == 0)	// 50 Hz
		{
			set_BNO055_TRIG;
			app_regs.REG_STREAM_MOTION = motion_counter;
			core_func_send_event(ADD_REG_STREAM_MOTION, false);
			motion_counter++;
		}
	}
	
	/* OXIMETER MAX220 reading */
 	if (app_regs.REG_STREAM_ENABLE & B_STREAM_OXIMETER)
	{
		if (ms_counter == 991 || ms_counter == 491)	// 2 Hz
			if (oximeter_read_all_step1(&oximeter) == 0)
			{
			// SUCCESS
			}
	
		if (ms_counter == 501 || ms_counter == 1)
			if (oximeter_read_all_step2(&oximeter, data) == 0)
			{
				// SUCCESS
			
				// Heart Rate
				//uint16_t heartRate;
				heartRate = data[0];
				heartRate = heartRate << 8;
				heartRate |= (data[1]); 
				heartRate /= 10; 

				// Confidence
				//uint8_t confidence;
				confidence = data[2];

				//Blood oxygen level
				//uint16_t oxygen;
				oxygen = data[3];
				oxygen = oxygen << 8;
				oxygen |= data[4]; 
				oxygen /= 10;

				//"Machine State" - has a finger been detected?
				status = data[5];
			
				app_regs.REG_STREAM_OXIMETER[0] = oxygen;
				app_regs.REG_STREAM_OXIMETER[1] = heartRate;
				app_regs.REG_STREAM_OXIMETER[2] = confidence;
				app_regs.REG_STREAM_OXIMETER[3] = status;
				core_func_send_event(ADD_REG_STREAM_OXIMETER, false);
			}
	}	
	
	/* MOTION SENSOR #1 BN055 reading */
	//if (app_regs.REG_STREAM_ENABLE & B_STREAM_MOTION)
	/*
	if(0)
	{
		if (ms_counter == 991 || ms_counter == 491)	// 2 Hz
		{
			bno055_read_vector(VECTOR_ACCELEROMETER, &app_regs.REG_STREAM_MOTION[0]);
			bno055_read_vector(VECTOR_MAGNETOMETER, &app_regs.REG_STREAM_MOTION[3]);
			bno055_read_vector(VECTOR_GYROSCOPE, &app_regs.REG_STREAM_MOTION[6]);
			bno055_read_vector(VECTOR_EULER, &app_regs.REG_STREAM_MOTION[9]);
			bno055_read_vector(VECTOR_LINEARACCEL, &app_regs.REG_STREAM_MOTION[12]);
			bno055_read_vector(VECTOR_GRAVITY, &app_regs.REG_STREAM_MOTION[15]);
			bno055_read_vector(VECTOR_QUATERNION, &app_regs.REG_STREAM_MOTION[18]);
				
			core_func_send_event(ADD_REG_STREAM_MOTION, true);				
		}
	}
	*/
	
	/* ECG Heart Rate ADC#5 reading*/
	if (app_regs.REG_STREAM_ENABLE & B_STREAM_ECG)
	{
		if ((ms_counter % 20) == 0)	// 50 Hz
		{
			app_regs.REG_STREAM_ECG[0] = adc_A_read_channel(5);
			if (app_regs.REG_STREAM_ECG[0] < 0)
				app_regs.REG_STREAM_ECG[0] = 0;
			
			app_regs.REG_STREAM_ECG[1] = adc_A_read_channel(1);
			if (app_regs.REG_STREAM_ECG[1] < 0)
				app_regs.REG_STREAM_ECG[1] = 0;
			
			core_func_send_event(ADD_REG_STREAM_ECG, false);
		}
		else
		{
			
			app_regs.REG_STREAM_ECG[1] = adc_A_read_channel(1);
			if (app_regs.REG_STREAM_ECG[1] < 0)
				app_regs.REG_STREAM_ECG[1] = 0;
			
			core_func_send_event(ADD_REG_STREAM_ECG, false);
		}
	}
	
	/* EDA/GSR module Mikroe-2860 ADC#7 reading */
	if (app_regs.REG_STREAM_ENABLE & B_STREAM_GSR)
	{
		if ((ms_counter % 250) == 0)	// 4 Hz
		{
			app_regs.REG_STREAM_GSR = adc_A_read_channel(7);
			if (app_regs.REG_STREAM_GSR < 0)
				app_regs.REG_STREAM_GSR = 0;
			
			core_func_send_event(ADD_REG_STREAM_GSR, false);
		}
	}
	
	ms_counter++;

	/*
	while(0)
	{
		if (app_regs.REG_STREAM_ENABLE & B_STREAM_MOTION)
		{
			bno055_read_vector(VECTOR_ACCELEROMETER, &app_regs.REG_STREAM_MOTION[0]);
			bno055_read_vector(VECTOR_MAGNETOMETER, &app_regs.REG_STREAM_MOTION[3]);
			bno055_read_vector(VECTOR_GYROSCOPE, &app_regs.REG_STREAM_MOTION[6]);
			bno055_read_vector(VECTOR_EULER, &app_regs.REG_STREAM_MOTION[9]);
			bno055_read_vector(VECTOR_LINEARACCEL, &app_regs.REG_STREAM_MOTION[12]);
			bno055_read_vector(VECTOR_GRAVITY, &app_regs.REG_STREAM_MOTION[15]);
			bno055_read_vector(VECTOR_QUATERNION, &app_regs.REG_STREAM_MOTION[18]);
		}
	}
	*/	
}

/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}