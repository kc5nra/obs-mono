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
	MonoObject *object;
	MonoMethod *load_method;
	MonoMethod *unload_method;
} mono;

bool obs_module_load(void)
{

//	mono_config_parse(NULL);
//	static const char* options[] = {
//		"--soft-breakpoints",
//		"--debugger-agent=transport=dt_socket,address=127.0.0.1:10000"
//        };
//
//	mono_jit_parse_options(sizeof(options)/sizeof(char*), (char**)options);
//	mono_debug_init(MONO_DEBUG_FORMAT_MONO);

	MonoDomain *domain = mono_jit_init_version("obs-mono", "v4.0.30319");

	if (!domain) {
		blog(LOG_ERROR, "obs-clr: unable to create 4.x domain");
		return false;
	}

#define MONO_CHECK(x, err) \
	if (!x) {\
		blog(LOG_ERROR, "obs-clr: " err); \
		goto error; \
	}

	MonoAssembly *assembly =
		mono_domain_assembly_open(domain,
			"/Users/jrb/Development/obs/obs-studio/plugins/"
			"obs-mono/CLR.Interop/bin/Debug/CLR.Interop.dll");
	MONO_CHECK(assembly, "unable to load CLR.Interop assembly");

	MonoImage *image = mono_assembly_get_image(assembly);
	MONO_CHECK(image, "unable to get image from assembly");

	MonoClass *class = mono_class_from_name(image,
		"CLR.Interop", "CLRHost");
	MONO_CHECK(class, "unable to find CLR.Interop.CLRHost class");

	MonoObject *object = mono_object_new(domain, class);
	MONO_CHECK(object, "unable to create new CLRHost object");

	mono_runtime_object_init(object);

	MonoMethod *load_method = mono_class_get_method_from_name(class, "Load", 0);
	MONO_CHECK(load_method, "unable to find CLRHost.Load method");
	MonoMethod *unload_method = mono_class_get_method_from_name(class,
			"Unload", 0);
	MONO_CHECK(unload_method, "unable to find CLRHost.Unload method");

#undef MONO_CHECK

	goto success;

error:
	mono_runtime_cleanup(mono.domain);
	return false;
success:
	mono.domain = domain;
	mono.object = object;
	mono.load_method = load_method;
	mono.unload_method = unload_method;

	mono_runtime_invoke(mono.load_method, mono.object, NULL, NULL);

	return true;
}

void obs_module_unload()
{
	mono_runtime_invoke(mono.unload_method, mono.object, NULL, NULL);
	mono_runtime_cleanup(mono.domain);
}