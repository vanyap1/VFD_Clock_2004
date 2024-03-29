/*
 * vfd_driver.c
 *
 * Created: 23.04.2023 16:53:37
 *  Author: Ivan Prints
 */ 

#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include "../config.h"
#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdbool.h>

enum port_pull_mode {
	PORT_PULL_OFF,
	PORT_PULL_UP,
};

enum port_dir {
	PORT_DIR_IN,
	PORT_DIR_OUT,
	PORT_DIR_OFF,
};


typedef struct
{
	uint8_t *port;
	uint8_t pin ;
} gpio;


void set_pin_level(gpio *GPIOx, const bool level);

void set_port_level(gpio *GPIOx, const uint8_t mask);

void set_pin_dir(gpio *GPIOx, const enum port_dir direction);

void set_pin_pull_mode(gpio *GPIOx, const enum port_pull_mode pull_mode);

void set_port_dir(gpio *GPIOx, const uint8_t mask);

static inline uint8_t get_port_level(gpio *GPIOx){
	uint8_t *tmp_port = GPIOx->port-2;
	return *tmp_port;
}



static inline bool get_port_pin_level(gpio *GPIOx)
{
	uint8_t *tmp_port = GPIOx->port-2;
	return *tmp_port & (1 << GPIOx->pin);
}

void toggle_pin_level(gpio *GPIOx);
void toggle_bit_level(gpio *GPIOx);

