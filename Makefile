CSC = gmcs
OUT_DIR = bin/

SRC = src/
SM_SRC = $(SRC)StateMachine/
BS_SRC = $(SRC)BootStrapper/
SAMPLE_SRC = $(SRC)Samples/
CONSOLE_SAM = $(SAMPLE_SRC)ConsoleSamurai/

TESTS_SRC = tests/
SM_TESTS =  $(TESTS_SRC)StateMachine/
BS_TESTS =  $(TESTS_SRC)BootStrapper/

DLLS = StateMachine.dll NinjectBootStrapper.dll
SPECS = StateMachine_specs.dll NinjectBootStrapper_specs.dll
SAMPLES = ConsoleSamurai.exe

project: make_folders $(DLLS)
samples: make_folders $(SAMPLES)
all: make_folders $(DLLS) $(SPECS) $(SAMPLES)

ConsoleSamurai.exe: $(DLLS) $(CONSOLE_SAM)ConsoleSamurai.cs $(CONSOLE_SAM)Contexts.cs \
					$(CONSOLE_SAM)Samurai.cs
	$(CSC) /out:$(OUT_DIR)Samples/ConsoleSamurai.exe \
		   $(CONSOLE_SAM)ConsoleSamurai.cs $(CONSOLE_SAM)Contexts.cs \
		   $(CONSOLE_SAM)Samurai.cs \
		   /r:bin/StateMachine.dll \
		   /r:bin/NinjectBootStrapper.dll \
		   /r:bin/Ninject.dll
	cp lib/Ninject.dll $(OUT_DIR)\Samples
	cp bin/StateMachine.dll $(OUT_DIR)\Samples
	cp bin/NinjectBootStrapper.dll $(OUT_DIR)\Samples

NinjectBootStrapper.dll: StateMachine.dll NinjectBootStrapper_no_dep
NinjectBootStrapper_no_dep: $(BS_SRC)NinjectBootStrapper.cs $(BS_SRC)MainModule.cs
	$(CSC) /out:$(OUT_DIR)NinjectBootStrapper.dll /lib:lib /t:library \
		   $(BS_SRC)NinjectBootStrapper.cs $(BS_SRC)Context.cs $(BS_SRC)MainModule.cs \
		   /r:bin/Ninject.dll \
		   /r:bin/StateMachine.dll
	cp lib/Ninject.dll $(OUT_DIR)

StateMachine.dll: $(SM_SRC)StateMachine.cs $(SM_SRC)State.cs
	$(CSC) /out:$(OUT_DIR)StateMachine.dll /lib:lib /t:library \
		   $(SM_SRC)StateMachine.cs $(SM_SRC)State.cs $(SM_SRC)CreatePathExpression.cs

NinjectBootStrapper_specs.dll: StateMachine.dll NinjectBootStrapper_specs_no_dep
NinjectBootStrapper_specs_no_dep: NinjectBootStrapper_no_dep \
							      $(BS_TESTS)NinjectBootStrapper_specs.cs \
							      $(BS_TESTS)Context_specs.cs \
							      $(BS_TESTS)MainModule_specs.cs
	$(CSC) /out:$(OUT_DIR)NinjectBootStrapper_specs.dll /lib:lib /t:library \
		   $(BS_TESTS)NinjectBootStrapper_specs.cs $(BS_TESTS)Context_specs.cs \
		   $(BS_TESTS)MainModule_specs.cs \
		   /r:nunit.framework.dll \
		   /r:SpecUnit.dll \
		   /r:bin/Ninject.dll \
		   /r:bin/NinjectBootStrapper.dll \
		   /r:bin/StateMachine.dll

StateMachine_specs.dll: StateMachine.dll $(SM_TESTS)StateMachineSpecs.cs
	$(CSC) /out:$(OUT_DIR)StateMachine_specs.dll /lib:lib /t:library \
		   $(SM_TESTS)StateMachineSpecs.cs \
		   /r:bin/StateMachine.dll \
		   /r:nunit.framework.dll \
		   /r:SpecUnit.dll
	cp lib/SpecUnit.dll $(OUT_DIR)
	cp lib/nunit.framework.dll $(OUT_DIR)

make_folders:
	mkdir -p bin/Samples
