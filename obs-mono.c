#include <obs-module.h>

#include "mono/jit/jit.h"
#include "mono/metadata/assembly.h"
#include "mono/metadata/mono-debug.h"
#include "mono/metadata/mono-config.h"


OBS_DECLARE_MODULE()
OBS_MODULE_USE_DEFAULT_LOCALE("obs-mono", "en-US")

bool obs_module_load(void)
{
	mono_config_parse(NULL);

	static const char* options[] = {
		"--soft-breakpoints",
		"--debugger-agent=transport=dt_socket,address=127.0.0.1:10000"
        };

	mono_jit_parse_options(sizeof(options)/sizeof(char*), (char**)options);

	mono_debug_init(MONO_DEBUG_FORMAT_MONO);

	MonoDomain *domain = mono_jit_init_version("obs-mono", "v4.0.30128");
//	MonoDomain *domain = mono_jit_init("Mono.Interop");
	MonoAssembly *interopAssembly =
		mono_domain_assembly_open(domain,
			"/Users/jrb/Projects/Mono.Interop/Mono.Interop/bin/Debug/Mono.Interop.dll");

	MonoImage *image = mono_assembly_get_image(interopAssembly);

	MonoClass *interopClass = mono_class_from_name(image,
		"Mono.Interop", "MonoHost");

	MonoObject *interopObject = mono_object_new(domain, interopClass);

	mono_runtime_object_init(interopObject);

	
	return true;
}

void
obs_module_unload()
{
}