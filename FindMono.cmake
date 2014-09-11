# Once done these will be defined:
#
#  MONO_FOUND
#  MONO_INCLUDE_DIRS
#  MONO_LIBRARIES
#

if(MONO_INCLUDE_DIRS AND MONO_LIBRARIES)
	set(MONO_FOUND TRUE)
else()
	find_package(PkgConfig QUIET)
	if (PKG_CONFIG_FOUND)
		pkg_check_modules(_MONO mono-2)
	endif()

	find_path(MONO_INCLUDE_DIR
		NAMES mono/jit/jit.h
		HINTS
			${_MONO_INCLUDE_DIRS}
		PATHS
			/usr/include /usr/local/include /opt/local/include)

	find_library(MONO_LIB
		NAMES mono-2.0
		HINTS
			${_MONO_LIBRARY_DIRS}
		PATHS
			/usr/lib /usr/local/lib /opt/local/lib)

	set(MONO_INCLUDE_DIRS ${MONO_INCLUDE_DIR}
		CACHE PATH "MONO include dir")
	set(MONO_LIBRARIES "${MONO_LIB}"
		CACHE STRING "MONO libraries")

	include(FindPackageHandleStandardArgs)
	find_package_handle_standard_args(MONO DEFAULT_MSG MONO_LIB
		MONO_INCLUDE_DIR)
	mark_as_advanced(MONO_INCLUDE_DIR MONO_LIB)
endif()
