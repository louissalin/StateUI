cdd.dll: src/StateMachine.cs
	gmcs /out:bin/cdd.dll /lib:lib /t:library src/StateMachine.cs /r:Ninject.dll

cdd_specs.dll: cdd.dll tests/StateMachineSpecs.cs
	gmcs /out:bin/cdd_specs.dll /lib:lib /t:library tests/StateMachineSpecs.cs /r:bin/cdd.dll /r:nunit.framework.dll /r:SpecUnit.dll

all: cdd.dll cdd_specs.dll
	cp lib/*.dll bin
