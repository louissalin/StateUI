CSC = gmcs
OUT_DIR = bin/
SRC = src/
TESTS_SRC = tests/

DLLS = StateMachine.dll
SPECS = StateMachine_specs.dll
ALL = $(DLLS) $(SPECS)

project: $(DLLS)
all: $(ALL)

StateMachine.dll: $(SRC)StateMachine.cs
	$(CSC) /out:$(OUT_DIR)StateMachine.dll /lib:lib /t:library \
		   $(SRC)StateMachine.cs \
		   /r:Ninject.dll
	cp lib/Ninject.dll bin

StateMachine_specs.dll: StateMachine.dll $(TESTS_SRC)StateMachineSpecs.cs
	$(CSC) /out:$(OUT_DIR)StateMachine_specs.dll /lib:lib /t:library \
		   $(TESTS_SRC)StateMachineSpecs.cs \
		   /r:bin/StateMachine.dll \
		   /r:nunit.framework.dll \
		   /r:SpecUnit.dll
	cp lib/SpecUnit.dll bin
	cp lib/nunit.framework.dll bin

