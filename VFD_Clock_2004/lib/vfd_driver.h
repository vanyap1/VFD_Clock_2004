/*
 * vfd_driver.h
 *
 * Created: 23.04.2023 16:53:58
 *  Author: Ivan Prints
 */ 

#include <string.h>#include <stdio.h>
#include "../config.h"

#ifndef VFD_DRIVER_H
#define VFD_DRIVER_H

#include <stdint.h>

typedef struct {
	char first_line[21];
	char second_line[21];
} display;


//extern void parseString(const char* input, display* output);






void spiwrite(uint8_t c);
void vfd_command(uint8_t c);
void vfd_data(uint8_t c);
void vfd_init();
void vfd_string(uint8_t *c);
void vfd_set_cursor(uint8_t line, uint8_t position);

#endif  // VFD_DRIVER_H