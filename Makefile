CSC = gmcs
DLLS = stateMachine.dll
SPECS = stateMachine_specs.dll
ALL = $(DLLS) $(SPECS)

project: $(DLLS)
all: $(ALL)

stateMachine.dll: src/StateMachine.cs
	$(CSC) /out:bin/StateMachine.dll /lib:lib /t:library src/StateMachine.cs \
		   /r:Ninject.dll
	cp lib/Ninject.dll bin

stateMachine_specs.dll: stateMachine.dll tests/StateMachineSpecs.cs
	$(CSC) /out:bin/StateMachine_specs.dll /lib:lib /t:library tests/StateMachineSpecs.cs \
		 /r:bin/StateMachine.dll \
		 /r:nunit.framework.dll \
		 /r:SpecUnit.dll
	cp lib/SpecUnit.dll bin
	cp lib/nunit.framework.dll bin

