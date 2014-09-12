#include <obs-module.h>
#include <util/platform.h>

#include "mono/jit/jit.h"
#include "mono/metadata/assembly.h"
#include "mono/metadata/mono-debug.h"
#include "mono/metadata/mono-config.h"
#include "mono/metadata/threads.h"

#include <mach-o/dyld.h>
#include <mach-o/getsect.h>

OBS_DECLARE_MODULE()
OBS_MODULE_USE_DEFAULT_LOCALE("obs-mono", "en-US")

struct obs_mono {
	MonoDomain *domain;
} mono;

bool obs_module_load(void)
{
	//mono_config_parse(NULL);

//	static const char* options[] = {
//		"--soft-breakpoints",
//		"--debugger-agent=transport=dt_socket,address=127.0.0.1:10000"
//        };
//
//	mono_jit_parse_options(sizeof(options)/sizeof(char*), (char**)options);

//	mono_debug_init(MONO_DEBUG_FORMAT_MONO)

//
	MonoDomain *domain = mono_jit_init_version("obs-mono", "v4.0.30319");
	MonoAssembly *assembly =
		mono_domain_assembly_open(domain,
			"/Users/johnbradley/Development/Workspaces/obs/"
			"obs-studio/plugins/obs-mono/Mono.Interop/bin/"
			"Debug/Mono.Interop.dll");

	MonoImage *image = mono_assembly_get_image(assembly);

	MonoClass *class = mono_class_from_name(image,
		"Mono.Interop", "MonoHost");

	MonoObject *object = mono_object_new(domain, class);
	mono_runtime_object_init(object);

	mono.domain = domain;

	return true;
}

bool
obs_module_unload()
{
	mono_runtime_cleanup(mono.domain);
	return false;
}