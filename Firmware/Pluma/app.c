#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#include "oximeter_hrsensor.h"
#include "i2c_oximeter.h"
#include "i2c_bno055.h"


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
    uint8_t fwL = 1;
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
	initialize_bno055();
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	
}

void core_callback_registers_were_reinitialized(void)
{
	/* Update registers if needed */
	
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

void core_callback_t_before_exec(void) {}
void core_callback_t_after_exec(void) {}

void core_callback_t_new_second(void)
{
	ms_counter = 0;
}

void core_callback_t_500us(void) {}

void core_callback_t_1ms(void)
{
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
				core_func_send_event(ADD_REG_STREAM_OXIMETER, true);
			}
	}
	
	if (app_regs.REG_STREAM_ENABLE & B_STREAM_ECG)
	{
		if ((ms_counter % 20) == 0)	// 50 Hz
		{
			app_regs.REG_STREAM_ECG = adc_A_read_channel(5);
			if (app_regs.REG_STREAM_ECG < 0)
				app_regs.REG_STREAM_ECG = 0;
			
			core_func_send_event(ADD_REG_STREAM_ECG, true);
		}
	}
		
	if (app_regs.REG_STREAM_ENABLE & B_STREAM_GSR)
	{
		if ((ms_counter % 250) == 0)	// 4 Hz
		{
			app_regs.REG_STREAM_GSR = adc_A_read_channel(7);
			if (app_regs.REG_STREAM_GSR < 0)
				app_regs.REG_STREAM_GSR = 0;
			
			core_func_send_event(ADD_REG_STREAM_GSR, true);
		}
	}
	
	ms_counter++;
	
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
			 
	while(0)
	{
		int16_t ecg = adc_A_read_channel(5);
		if (ecg < 0)
			ecg = 0;
	
		app_regs.REG_STREAM_ECG = ecg;
		core_func_send_event(ADD_REG_STREAM_ECG, true);
	
	
		uint16_t gsr = adc_A_read_channel(7);
		if (gsr < 0)
		gsr = 0;
		
		app_regs.REG_STREAM_GSR = gsr;
		core_func_send_event(ADD_REG_STREAM_GSR, true);
	
		//_delay_ms(10);
	}
		


	//Oxygen & Heart Rate reading
	while(0)
	{	
		
		// This function reads the CONFIGURATION_REGISTER (0x0A)	
		//uint8_t configuration_register;
		//if (read_byte(&oximeter, READ_REGISTER, READ_MAX30101, CONFIGURATION_REGISTER, &configuration_register) == 0)
			//{ // SUCCESS
			 //}
		
		// Read all

		
		if (oximeter_read_all(&oximeter, data) == 0)
			{ // SUCCESS
			 }
		
		// Heart Rate formatting
		//uint16_t heartRate;
		heartRate = data[0];
		heartRate = heartRate << 8;
		heartRate |= (data[1]); 
		heartRate /= 10; 

		// Confidence formatting
		//uint8_t confidence;
		confidence = data[2];

		//Blood oxygen level formatting
		//uint16_t oxygen;
		oxygen = data[3];
		oxygen = oxygen << 8;
		oxygen |= data[4]; 
		oxygen /= 10;

		//"Machine State" - has a finger been detected?
		status = data[5];
	
		_delay_ms(100);
	}

	
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