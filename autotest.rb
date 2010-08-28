watch('.*.cs$') do |match|
	if system('make all') 
		system 'nunit-console2 bin/cdd_specs.dll'
	end
end
