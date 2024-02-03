
/*
 * spi_hall.h
 *
 * Created: 08.05.2023 19:17:27
 *  Author: User
 */ 
#ifndef SPI1_HAL_H_
#define SPI1_HAL_H_


#include <stdint.h>
#include "../config.h"
#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>

//void spi_master_init(void);
//void spi_tranfer(uint8_t *tx,uint8_t *rx,uint16_t len);





#endif /* SPI1_HAL_H_ */


void spi1_init();
void soft_spi1_init();

//void soft_spi_write_byte(uint8_t data);
void spi1_write_byte(uint8_t data);

uint8_t spi1_read_byte(void);
uint8_t soft_spi1_read_byte(void);
void spi1_tranfer(uint8_t *tx,uint16_t len);
void spi1_send(uint8_t *tx);