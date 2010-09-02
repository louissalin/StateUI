CSC = gmcs
OUT_DIR = bin/

SRC = src/
SM_SRC = $(SRC)StateMachine/
BS_SRC = $(SRC)BootStrapper/

TESTS_SRC = tests/
SM_TESTS =  $(TESTS_SRC)StateMachine/
BS_TESTS =  $(TESTS_SRC)BootStrapper/

DLLS = StateMachine.dll NinjectBootStrapper.dll
SPECS = StateMachine_specs.dll NinjectBootStrapper_specs.dll
ALL = $(DLLS) $(SPECS)

project: $(DLLS)
all: $(ALL)

NinjectBootStrapper.dll: StateMachine.dll $(BS_SRC)NinjectBootStrapper.cs
	$(CSC) /out:$(OUT_DIR)NinjectBootStrapper.dll /lib:lib /t:library \
		   $(BS_SRC)NinjectBootStrapper.cs \
		   /r:bin/Ninject.dll \
		   /r:bin/StateMachine.dll
	cp lib/Ninject.dll $(OUT_DIR)

StateMachine.dll: $(SM_SRC)StateMachine.cs $(SM_SRC)State.cs
	$(CSC) /out:$(OUT_DIR)StateMachine.dll /lib:lib /t:library \
		   $(SM_SRC)StateMachine.cs $(SM_SRC)State.cs $(SM_SRC)CreatePathExpression.cs \
		   $(BS_SRC)Context.cs 

NinjectBootStrapper_specs.dll: NinjectBootStrapper.dll \
							   $(BS_TESTS)NinjectBootStrapper_specs.cs
	$(CSC) /out:$(OUT_DIR)NinjectBootStrapper_specs.dll /lib:lib /t:library \
		   $(BS_TESTS)NinjectBootStrapper_specs.cs \
		   /r:nunit.framework.dll \
		   /r:SpecUnit.dll

StateMachine_specs.dll: StateMachine.dll $(SM_TESTS)StateMachineSpecs.cs
	$(CSC) /out:$(OUT_DIR)StateMachine_specs.dll /lib:lib /t:library \
		   $(SM_TESTS)StateMachineSpecs.cs \
		   /r:bin/StateMachine.dll \
		   /r:nunit.framework.dll \
		   /r:SpecUnit.dll
	cp lib/SpecUnit.dll $(OUT_DIR)
	cp lib/nunit.framework.dll $(OUT_DIR)

