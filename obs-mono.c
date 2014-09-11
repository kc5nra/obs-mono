#include <obs-module.h>

#include "mono/jit/jit.h"
#include "mono/metadata/mono-config.h"


OBS_DECLARE_MODULE()
OBS_MODULE_USE_DEFAULT_LOCALE("obs-mono", "en-US")

bool obs_module_load(void)
{
	mono_config_parse(NULL);
	MonoDomain *domain = mono_jit_init_version("obs-mono", "v4.0.30319");

	return true;
}

void
obs_module_unload()
{
}