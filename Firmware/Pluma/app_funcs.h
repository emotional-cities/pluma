#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_STREAM_ENABLE(void);
void app_read_REG_STREAM_DISABLE(void);
void app_read_REG_STREAM_OXIMETER(void);
void app_read_REG_STREAM_ECG(void);
void app_read_REG_STREAM_GSR(void);
void app_read_REG_STREAM_MOTION(void);

bool app_write_REG_STREAM_ENABLE(void *a);
bool app_write_REG_STREAM_DISABLE(void *a);
bool app_write_REG_STREAM_OXIMETER(void *a);
bool app_write_REG_STREAM_ECG(void *a);
bool app_write_REG_STREAM_GSR(void *a);
bool app_write_REG_STREAM_MOTION(void *a);


#endif /* _APP_FUNCTIONS_H_ */