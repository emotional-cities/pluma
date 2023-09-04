#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
// 
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
// 
// ISR(TCD1_OVF_vect, ISR_NAKED)
// 
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/ 
/* Digital input 0                                                      */
/************************************************************************/
ISR(PORTC_INT0_vect, ISR_NAKED)
{
	if (read_DI0)
	{
		app_regs.REG_INPUTS |= B_DI0;
		core_func_send_event(ADD_REG_INPUTS, true);
	}
	else
	{
		app_regs.REG_INPUTS &= ~(B_DI0);
		core_func_send_event(ADD_REG_INPUTS, true);
	}
	
	reti();
}

/************************************************************************/ 
/* Digital input 1                                                      */
/************************************************************************/
ISR(PORTC_INT1_vect, ISR_NAKED)
{
	if (read_DI1)
	{
		app_regs.REG_INPUTS |= B_DI1;
		core_func_send_event(ADD_REG_INPUTS, true);
	}
	else
	{		
		app_regs.REG_INPUTS &= ~(B_DI1);
		core_func_send_event(ADD_REG_INPUTS, true);
	}
	
	reti();
}

