project(obs-mono)

set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${CMAKE_CURRENT_SOURCE_DIR}")
set(CMAKE_INCLUDE_CURRENT_DIR TRUE)

find_package(LibObs REQUIRED)
find_package(Mono REQUIRED)

message("Mono: ${MONO_INCLUDE_DIRS}")

set(obs-mono_SOURCES
	obs-mono.c)

add_library(obs-mono MODULE
	${obs-mono_SOURCES})

target_include_directories(obs-mono SYSTEM PRIVATE
	${MONO_INCLUDE_DIRS})

target_link_libraries(obs-mono
	libobs
	${MONO_LIB})

install_obs_plugin_with_data(obs-mono data)
